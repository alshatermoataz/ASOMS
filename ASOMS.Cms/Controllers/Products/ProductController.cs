using ASOMS.Cms.Services.Interfaces;
using ASOMS.Cms.Services.ProductServices;
using ASOMS.Core.DTOs.Products;
using ASOMS.DAL.EntityFramework;
using ASOMS.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASOMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController (CustomDbContext customDbContext , IWebHostEnvironment env, IProductService _productService, ILogger<ProductService> _logger) : ControllerBase
    {

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetAll([FromQuery] ProductQueryParameters parameters)
        {
            try
            {
                var result = await _productService.GetProductsAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            var product = await customDbContext.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return product;
        }


            [HttpPost]
            [Route("upload")]
            public async Task<IActionResult> UploadProduct([FromForm] ProductUploadDto dto)
            {
                if (dto.Image == null || dto.Image.Length == 0)
                    return BadRequest("Image file is required.");

                var imageFileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var imagePath = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", imageFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    Brand = dto.Brand,
                    Category = dto.Category,
                    Size = dto.Size,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    ImageUrl = $"/uploads/{imageFileName}"
                };

                customDbContext.Products.Add(product);
                await customDbContext.SaveChangesAsync();

                return Ok(product);
            }

        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadProductImage(Guid id, IFormFile image)
        {
            var product = await customDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            if (image == null || image.Length == 0)
                return BadRequest("No image uploaded");

            var imageFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var uploadsFolder = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, imageFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            product.ImageUrl = $"/uploads/{imageFileName}";
            await customDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPost("bulk-upload")]
        public async Task<IActionResult> BulkUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // Check file extension
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (fileExtension != ".csv" && fileExtension != ".xlsx" && fileExtension != ".xls")
                return BadRequest("Only CSV and Excel files are supported");

            try
            {
                var result = await _productService.ProcessBulkUpload(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing bulk upload");
                return StatusCode(500, "An error occurred while processing the file");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productDto)
        {
            if (productDto == null)
                return BadRequest("Product data is required");

            try
            {
                var result = await _productService.UpdateProductDetailsAsync(id, productDto);
                
                if (result == null)
                    return NotFound($"Product with ID {id} not found");
                
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid product update attempt");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return StatusCode(500, "An error occurred while updating the product");
            }
        }
    }
}
