using ASOMS.Cms.Services.Interfaces;
using ASOMS.Core.DTOs.Products;
using ASOMS.DAL.EntityFramework;
using ASOMS.DAL.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ASOMS.Core.DTOs;
using ASOMS.DAL.Models;

namespace ASOMS.Cms.Services.ProductServices
{
    public class ProductService(
        CustomDbContext customDbContext , ILogger<ProductService> _logger) : IProductService
    {
        private readonly bool _allowPartialImports = true; // Set to false if you want all-or-nothing imports
        private readonly HashSet<string> _validCategories = new HashSet<string>
        {
            "Tyres", "Batteries", "Oil", "Parts", "Accessories"
        };
        private const int BatchSize = 100;

        #region Product CRUD Operations
        // Your existing product CRUD methods would go here
        // GetProducts, GetProductById, CreateProduct, UpdateProduct, DeleteProduct, etc.
        #endregion

        #region Bulk Upload Processing

        /// <summary>
        /// Process a bulk upload file (CSV or Excel) and import products
        /// </summary>
        public async Task<BulkUploadResultDto> ProcessBulkUpload(IFormFile file)
        {
            _logger.LogInformation("Starting bulk upload process for file: {FileName}, Size: {FileSize}",
                file.FileName, file.Length);

            var result = new BulkUploadResultDto
            {
                TotalRecords = 0,
                SuccessfulRecords = 0,
                FailedRecords = 0,
                Errors = new List<string>(),
                FileName = file.FileName,
                ImportDate = DateTime.UtcNow
            };

            // Validate file
            if (!IsValidFileType(file))
            {
                result.Errors.Add("Invalid file type. Only CSV and Excel files are supported.");
                return result;
            }

            try
            {
                // Use transaction for data consistency
                using (var transaction = await customDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Process based on file type
                        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        if (fileExtension == ".csv")
                        {
                            await ProcessCsvFile(file, result);
                        }
                        else if (fileExtension == ".xlsx" || fileExtension == ".xls")
                        {
                            await ProcessExcelFile(file, result);
                        }

                        // Commit transaction if successful or partial imports are allowed
                        if (result.FailedRecords == 0 || _allowPartialImports)
                        {
                            await transaction.CommitAsync();
                            _logger.LogInformation("Bulk upload completed successfully. Total: {Total}, Success: {Success}, Failed: {Failed}",
                                result.TotalRecords, result.SuccessfulRecords, result.FailedRecords);
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            result.SuccessfulRecords = 0;
                            result.Errors.Add("Transaction rolled back due to errors. No records were imported.");
                            _logger.LogWarning("Bulk upload transaction rolled back. Total errors: {ErrorCount}", result.Errors.Count);
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error during bulk upload transaction. Transaction rolled back.", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing bulk upload");
                result.Errors.Add($"Error processing file: {ex.Message}");
                if (ex.InnerException != null)
                {
                    result.Errors.Add($"Additional details: {ex.InnerException.Message}");
                }
            }

            return result;
        }

        /// <summary>
        /// Process a CSV file for bulk product import
        /// </summary>
        private async Task ProcessCsvFile(IFormFile file, BulkUploadResultDto result)
        {
            _logger.LogInformation("Processing CSV file: {FileName}", file.FileName);

            // For large files, use the batch processing method
            if (file.Length > 5 * 1024 * 1024) // 5MB
            {
                await ProcessLargeCsvFile(file, result);
                return;
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null
            }))
            {
                try
                {
                    // Read all records
                    var records = csv.GetRecords<ProductImportModel>().ToList();
                    result.TotalRecords = records.Count;
                    _logger.LogInformation("Read {Count} records from CSV file", records.Count);

                    // Process each record
                    foreach (var record in records)
                    {
                        try
                        {
                            // Sanitize inputs
                            SanitizeProductRecord(record);

                            // Validate record
                            if (!ValidateRecord(record, result))
                            {
                                continue;
                            }

                            // Create product entity
                            var product = MapToProduct(record);
                            await customDbContext.Products.AddAsync(product);
                            result.SuccessfulRecords++;
                        }
                        catch (Exception ex)
                        {
                            HandleRecordError(ex, record, result);
                        }
                    }

                    // Save changes
                    await customDbContext.SaveChangesAsync();
                }
                catch (CsvHelper.MissingFieldException ex)
                {
                    _logger.LogError(ex, "CSV missing field error");
                    result.Errors.Add($"CSV format error: Missing field at row {ex.Context?.Parser?.Row}");
                    throw;
                }
                catch (CsvHelper.HeaderValidationException ex)
                {
                    _logger.LogError(ex, "CSV header validation error");
                    result.Errors.Add($"CSV header error: {ex.Message}. Expected headers: Name, Category, Brand, Description, Price, Quantity, Size");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing CSV file");
                    result.Errors.Add($"Error processing CSV file: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Process a large CSV file in batches to avoid memory issues
        /// </summary>
        private async Task ProcessLargeCsvFile(IFormFile file, BulkUploadResultDto result)
        {
            _logger.LogInformation("Processing large CSV file in batches: {FileName}", file.FileName);

            var products = new List<Product>(BatchSize);
            int totalProcessed = 0;

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null
            }))
            {
                // Read header
                csv.Read();
                csv.ReadHeader();

                // Read records in batches
                while (csv.Read())
                {
                    totalProcessed++;
                    result.TotalRecords = totalProcessed;

                    try
                    {
                        // Parse record
                        var record = new ProductImportModel
                        {
                            Name = csv.GetField("Name"),
                            Category = csv.GetField("Category"),
                            Brand = csv.GetField("Brand"),
                            Description = csv.GetField("Description"),
                            Price = csv.GetField<decimal>("Price"),
                            Quantity = csv.GetField<int>("Quantity"),
                            Size = csv.GetField("Size")
                        };

                        // Sanitize inputs
                        SanitizeProductRecord(record);

                        // Validate and create product
                        if (ValidateRecord(record, result))
                        {
                            var product = MapToProduct(record);
                            products.Add(product);
                            result.SuccessfulRecords++;
                        }

                        // Process in batches
                        if (products.Count >= BatchSize)
                        {
                            await SaveBatch(products);
                            _logger.LogInformation("Saved batch of {Count} products", products.Count);
                            products.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error at row {totalProcessed}: {ex.Message}");
                        _logger.LogError(ex, "Error processing CSV row {Row}", totalProcessed);
                    }
                }

                // Save any remaining products
                if (products.Any())
                {
                    await SaveBatch(products);
                    _logger.LogInformation("Saved final batch of {Count} products", products.Count);
                }
            }
        }

        /// <summary>
        /// Process an Excel file for bulk product import
        /// </summary>
        private async Task ProcessExcelFile(IFormFile file, BulkUploadResultDto result)
        {
            _logger.LogInformation("Processing Excel file: {FileName}", file.FileName);

            // Set EPPlus license context
            // Change this line in the ProcessExcelFile method:
            // Change this line in the ProcessExcelFile method:
            //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

            ExcelPackage.License.SetNonCommercialPersonal("ASOMS");

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;

                    if (rowCount <= 1)
                    {
                        result.Errors.Add("Excel file is empty or contains only headers.");
                        return;
                    }

                    // Skip header row
                    result.TotalRecords = rowCount - 1;
                    _logger.LogInformation("Found {Count} rows in Excel file", result.TotalRecords);

                    // Process in batches for large files
                    var products = new List<Product>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var record = new ProductImportModel
                            {
                                Name = GetExcelCellValue(worksheet, row, 1),
                                Category = GetExcelCellValue(worksheet, row, 2),
                                Brand = GetExcelCellValue(worksheet, row, 3),
                                Description = GetExcelCellValue(worksheet, row, 4),
                                Price = ParseDecimal(GetExcelCellValue(worksheet, row, 5)),
                                Quantity = ParseInt(GetExcelCellValue(worksheet, row, 6)),
                                Size = GetExcelCellValue(worksheet, row, 7)
                            };

                            // Sanitize inputs
                            SanitizeProductRecord(record);

                            // Validate record
                            if (!ValidateRecord(record, result))
                            {
                                continue;
                            }

                            // Create product entity
                            var product = MapToProduct(record);
                            products.Add(product);
                            result.SuccessfulRecords++;

                            // Save in batches
                            if (products.Count >= BatchSize)
                            {
                                await customDbContext.Products.AddRangeAsync(products);
                                await customDbContext.SaveChangesAsync();
                                _logger.LogInformation("Saved batch of {Count} products from Excel", products.Count);
                                products.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            result.FailedRecords++;
                            result.Errors.Add($"Error at row {row}: {ex.Message}");
                            _logger.LogError(ex, "Error processing Excel row {Row}", row);
                        }
                    }

                    // Save any remaining products
                    if (products.Any())
                    {
                        await customDbContext.Products.AddRangeAsync(products);
                        await customDbContext.SaveChangesAsync();
                        _logger.LogInformation("Saved final batch of {Count} products from Excel", products.Count);
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Save a batch of products to the database
        /// </summary>
        private async Task SaveBatch(List<Product> products)
        {
            await customDbContext.Products.AddRangeAsync(products);
            await customDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Map a ProductImportModel to a Product entity
        /// </summary>
        private Product MapToProduct(ProductImportModel record)
        {
            return new Product
            {
                Name = record.Name,
                Description = record.Description,
                Category = record.Category,
                Brand = record.Brand,
                Size = record.Size,
                Price = record.Price,
                Quantity = record.Quantity,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Validate a product record
        /// </summary>
        private bool ValidateRecord(ProductImportModel record, BulkUploadResultDto result)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(record.Name))
                errors.Add("Product name is required");

            if (string.IsNullOrWhiteSpace(record.Category))
                errors.Add("Category is required");

            // Numeric validations
            if (record.Price <= 0)
                errors.Add("Price must be greater than zero");

            if (record.Quantity < 0)
                errors.Add("Quantity cannot be negative");

            // Category validation
            if (!string.IsNullOrWhiteSpace(record.Category) && !_validCategories.Contains(record.Category))
                errors.Add($"Invalid category: {record.Category}. Valid categories are: {string.Join(", ", _validCategories)}");

            // Check for duplicate product name
            if (!string.IsNullOrWhiteSpace(record.Name) &&
                customDbContext.Products.Any(p => p.Name == record.Name))
            {
                errors.Add($"A product with the name '{record.Name}' already exists");
            }

            if (errors.Any())
            {
                result.FailedRecords++;
                result.Errors.Add($"Validation failed for product '{record.Name ?? "Unknown"}': {string.Join(", ", errors)}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handle errors when processing a record
        /// </summary>
        private void HandleRecordError(Exception ex, ProductImportModel record, BulkUploadResultDto result)
        {
            result.FailedRecords++;
            var productName = record?.Name ?? "Unknown";
            result.Errors.Add($"Error processing product '{productName}': {ex.Message}");
            _logger.LogError(ex, "Error processing product record: {ProductName}", productName);
        }

        /// <summary>
        /// Sanitize product record inputs to prevent security issues
        /// </summary>
        private void SanitizeProductRecord(ProductImportModel record)
        {
            if (record == null) return;

            record.Name = SanitizeInput(record.Name);
            record.Description = SanitizeInput(record.Description);
            record.Brand = SanitizeInput(record.Brand);
            record.Category = SanitizeInput(record.Category);
            record.Size = SanitizeInput(record.Size);
        }

        /// <summary>
        /// Sanitize input string to prevent XSS and injection attacks
        /// </summary>
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Remove potentially dangerous HTML/script tags
            input = Regex.Replace(input, @"<.*?>", string.Empty);

            // Limit length
            if (input.Length > 500)
                input = input.Substring(0, 500);

            return input;
        }

        /// <summary>
        /// Check if the file is a valid type for import
        /// </summary>
        private bool IsValidFileType(IFormFile file)
        {
            if (file == null)
                return false;

            // Check extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension == ".csv" || extension == ".xlsx" || extension == ".xls";
        }

        /// <summary>
        /// Get cell value from Excel worksheet as string
        /// </summary>
        private string GetExcelCellValue(ExcelWorksheet worksheet, int row, int column)
        {
            return worksheet.Cells[row, column]?.Value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Parse decimal value from string with error handling
        /// </summary>
        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                return result;

            throw new FormatException($"Invalid decimal value: {value}");
        }

        /// <summary>
        /// Parse integer value from string with error handling
        /// </summary>
        private int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (int.TryParse(value, out int result))
                return result;

            throw new FormatException($"Invalid integer value: {value}");
        }


        public async Task<Product> UpdateProductDetailsAsync(Guid id, ProductUpdateDto productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            // Validate product data
            ValidateProductData(productDto);

            // Find the product
            var product = await customDbContext.Products.FindAsync(id);
            if (product == null)
                return null;

            // Update product properties
            product.Name = SanitizeInput(productDto.Name);
            product.Description = SanitizeInput(productDto.Description);
            product.Brand = SanitizeInput(productDto.Brand);
            product.Category = SanitizeInput(productDto.Category);
            product.Size = SanitizeInput(productDto.Size);
            product.Price = productDto.Price;
            product.Quantity = productDto.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            // Save changes
            try
            {
                await customDbContext.SaveChangesAsync();
                _logger.LogInformation("Product {ProductId} updated successfully", id);
                return product;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict updating product {ProductId}", id);
                throw new Exception("The product was modified by another user. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating product {ProductId}", id);
                throw new Exception("An error occurred while saving the product. Please try again.", ex);
            }
        }

        private void ValidateProductData(ProductUpdateDto productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.Name))
                throw new ArgumentException("Product name is required");

            if (string.IsNullOrWhiteSpace(productDto.Category))
                throw new ArgumentException("Category is required");

            if (productDto.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (productDto.Quantity < 0)
                throw new ArgumentException("Quantity cannot be negative");

            // Validate category is in the allowed list
            if (!_validCategories.Contains(productDto.Category))
                throw new ArgumentException($"Invalid category: {productDto.Category}. Valid categories are: {string.Join(", ", _validCategories)}");
        }

        #endregion


        // Services/ProductService.cs - Updated for your models

        /// <summary>
        /// More efficient and safer approach using joins
        /// </summary>
        public async Task<PagedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters)
        {
            _logger.LogInformation("Getting products with parameters: {@Parameters}", parameters);

            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            try
            {
                // First, get order activity summary
                var orderActivity = await customDbContext.OrderItems
                    .Where(oi => oi.Order.CreatedAt >= thirtyDaysAgo)
                    .GroupBy(oi => oi.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        LastOrderDate = g.Max(oi => oi.Order.CreatedAt),
                        OrderCount = g.Sum(oi => oi.Quantity),
                        Revenue = g.Sum(oi => oi.Quantity * oi.Price)
                    })
                    .ToListAsync();

                // Convert to dictionary for efficient lookup
                var activityDict = orderActivity.ToDictionary(a => a.ProductId);

                // Build main query
                var query = customDbContext.Products
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name ?? string.Empty,
                        Description = p.Description,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        ImageUrl = p.ImageUrl,
                        Brand = p.Brand,
                        Category = p.Category ?? string.Empty,
                        Size = p.Size,
                        Specifications = p.Specifications,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        Status = "Inactive", // Will be updated after query
                        LastOrderDate = null,
                        OrderCountLast30Days = 0,
                        RevenueLast30Days = 0
                    });

                // Apply filters (excluding status filter for now)
                query = ApplyFiltersToDto(query, parameters, excludeStatus: true);

                // Get total count before applying status filter
                var allProducts = await query.ToListAsync();

                // Update products with activity data
                foreach (var product in allProducts)
                {
                    if (activityDict.TryGetValue(product.Id, out var activity))
                    {
                        product.Status = "Active";
                        product.LastOrderDate = activity.LastOrderDate;
                        product.OrderCountLast30Days = activity.OrderCount;
                        product.RevenueLast30Days = activity.Revenue;
                    }
                }

                // Apply status filter if specified
                if (!string.IsNullOrWhiteSpace(parameters.Status))
                {
                    allProducts = allProducts
                        .Where(p => string.Equals(p.Status, parameters.Status, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var totalCount = allProducts.Count;

                // Apply sorting
                allProducts = ApplySortingToList(allProducts, parameters);

                // Apply pagination
                var skip = (parameters.Page - 1) * parameters.PageSize;
                var products = allProducts
                    .Skip(skip)
                    .Take(parameters.PageSize)
                    .ToList();

                var result = new PagedResult<ProductDto>
                {
                    Items = products,
                    TotalCount = totalCount,
                    Page = parameters.Page,
                    PageSize = parameters.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize),
                    HasNextPage = parameters.Page < (int)Math.Ceiling((double)totalCount / parameters.PageSize),
                    HasPreviousPage = parameters.Page > 1
                };

                _logger.LogInformation("Retrieved {Count} products out of {Total} total",
                    products.Count, totalCount);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProductsAsync: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Apply filters to ProductDto query with option to exclude status
        /// </summary>
        private IQueryable<ProductDto> ApplyFiltersToDto(IQueryable<ProductDto> query, ProductQueryParameters parameters, bool excludeStatus = false)
        {
            // Category filter
            if (!string.IsNullOrWhiteSpace(parameters.Category))
            {
                query = query.Where(p => p.Category == parameters.Category);
            }

            // Search filter
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var searchTerm = parameters.Search.ToLower();
                query = query.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(searchTerm)) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                    (p.Brand != null && p.Brand.ToLower().Contains(searchTerm)) ||
                    (p.Specifications != null && p.Specifications.ToLower().Contains(searchTerm)));
            }

            // Stock level filter
            if (!string.IsNullOrWhiteSpace(parameters.StockLevel))
            {
                switch (parameters.StockLevel.ToLower())
                {
                    case "low":
                        query = query.Where(p => p.Quantity < 10);
                        break;
                    case "medium":
                        query = query.Where(p => p.Quantity >= 10 && p.Quantity <= 50);
                        break;
                    case "high":
                        query = query.Where(p => p.Quantity > 50);
                        break;
                }
            }

            // Status filter (only if not excluded)
            if (!excludeStatus && !string.IsNullOrWhiteSpace(parameters.Status))
            {
                query = query.Where(p => p.Status == parameters.Status);
            }

            // Min/Max stock filters
            if (parameters.MinStock.HasValue)
            {
                query = query.Where(p => p.Quantity >= parameters.MinStock.Value);
            }

            if (parameters.MaxStock.HasValue)
            {
                query = query.Where(p => p.Quantity <= parameters.MaxStock.Value);
            }

            return query;
        }

        /// <summary>
        /// Apply sorting to ProductDto query
        /// </summary>
        private IQueryable<ProductDto> ApplySortingToDto(IQueryable<ProductDto> query, ProductQueryParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.SortBy))
                return query.OrderBy(p => p.Name);

            var isDescending = parameters.SortDirection?.ToLower() == "desc";

            return parameters.SortBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "quantity" or "stock" => isDescending ? query.OrderByDescending(p => p.Quantity) : query.OrderBy(p => p.Quantity),
                "brand" => isDescending ? query.OrderByDescending(p => p.Brand) : query.OrderBy(p => p.Brand),
                "category" => isDescending ? query.OrderByDescending(p => p.Category) : query.OrderBy(p => p.Category),
                "createdat" or "created" => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                "updatedat" or "updated" => isDescending ? query.OrderByDescending(p => p.UpdatedAt) : query.OrderBy(p => p.UpdatedAt),
                "lastorder" => isDescending ? query.OrderByDescending(p => p.LastOrderDate) : query.OrderBy(p => p.LastOrderDate),
                "status" => isDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
                "ordercount" => isDescending ? query.OrderByDescending(p => p.OrderCountLast30Days) : query.OrderBy(p => p.OrderCountLast30Days),
                "revenue" => isDescending ? query.OrderByDescending(p => p.RevenueLast30Days) : query.OrderBy(p => p.RevenueLast30Days),
                _ => query.OrderBy(p => p.Name)
            };
        }

        /// <summary>
        /// Apply sorting to a list of ProductDto
        /// </summary>
        private List<ProductDto> ApplySortingToList(List<ProductDto> products, ProductQueryParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.SortBy))
                return products.OrderBy(p => p.Name).ToList();

            var isDescending = parameters.SortDirection?.ToLower() == "desc";

            return parameters.SortBy.ToLower() switch
            {
                "name" => isDescending ? products.OrderByDescending(p => p.Name).ToList() : products.OrderBy(p => p.Name).ToList(),
                "price" => isDescending ? products.OrderByDescending(p => p.Price).ToList() : products.OrderBy(p => p.Price).ToList(),
                "quantity" or "stock" => isDescending ? products.OrderByDescending(p => p.Quantity).ToList() : products.OrderBy(p => p.Quantity).ToList(),
                "brand" => isDescending ? products.OrderByDescending(p => p.Brand).ToList() : products.OrderBy(p => p.Brand).ToList(),
                "category" => isDescending ? products.OrderByDescending(p => p.Category).ToList() : products.OrderBy(p => p.Category).ToList(),
                "createdat" or "created" => isDescending ? products.OrderByDescending(p => p.CreatedAt).ToList() : products.OrderBy(p => p.CreatedAt).ToList(),
                "updatedat" or "updated" => isDescending ? products.OrderByDescending(p => p.UpdatedAt).ToList() : products.OrderBy(p => p.UpdatedAt).ToList(),
                "lastorder" => isDescending ? products.OrderByDescending(p => p.LastOrderDate).ToList() : products.OrderBy(p => p.LastOrderDate).ToList(),
                "status" => isDescending ? products.OrderByDescending(p => p.Status).ToList() : products.OrderBy(p => p.Status).ToList(),
                "ordercount" => isDescending ? products.OrderByDescending(p => p.OrderCountLast30Days).ToList() : products.OrderBy(p => p.OrderCountLast30Days).ToList(),
                "revenue" => isDescending ? products.OrderByDescending(p => p.RevenueLast30Days).ToList() : products.OrderBy(p => p.RevenueLast30Days).ToList(),
                _ => products.OrderBy(p => p.Name).ToList()
            };
        }
    }
}