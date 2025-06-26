using ASOMS.Cms.Services.AnalyticsInterface;
using ASOMS.Core.DTOs.Analytics;
using ASOMS.DAL.EntityFramework;
using ASOMS.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASOMS.Cms.Services.AnalyticsService
{
    public class AnalyticsService(CustomDbContext customDbContext) : IAnalyticsService
    {
        public async Task<object> GetDashboardSummaryAsync(string period)
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);
            var previousStartDate = DateTime.UtcNow.AddDays(-days * 2);

            // Get orders with their items for better performance
            var currentOrders = await customDbContext.Orders
                .Include(o => o.Items)
                .Where(o => o.CreatedAt >= startDate)
                .ToListAsync();

            var previousOrders = await customDbContext.Orders
                .Include(o => o.Items)
                .Where(o => o.CreatedAt >= previousStartDate && o.CreatedAt < startDate)
                .ToListAsync();

            // Calculate metrics
            var metrics = CalculateMetrics(currentOrders, previousOrders);

            // Customer metrics
            var customerMetrics = await CalculateCustomerMetricsAsync(startDate, previousStartDate);

            return new
            {
                totalRevenue = metrics.TotalRevenue,
                revenueGrowth = metrics.RevenueGrowth,
                totalOrders = metrics.TotalOrders,
                ordersGrowth = metrics.OrdersGrowth,
                avgOrderValue = metrics.AvgOrderValue,
                totalCustomers = customerMetrics.TotalCustomers,
                customerGrowth = customerMetrics.CustomerGrowth,
                newCustomers = customerMetrics.NewCustomers,
                conversionRate = customerMetrics.ConversionRate,
                period
            };
        }

        // Replace the GetCustomerSegmentsAsync method with this corrected version:

        public async Task<List<object>> GetCustomerSegmentsAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            // Get all customers
            var totalCustomers = await customDbContext.Users.Where(x=> x.Role == "Customer").CountAsync();

            // Get customers with recent orders
            var activeCustomers = await customDbContext.Orders
                .Where(o => o.CreatedAt >= thirtyDaysAgo)
                .Select(o => o.UserId)
                .Distinct()
                .CountAsync();

            var inactiveCustomers = totalCustomers - activeCustomers;

            var segments = new List<object>();

            if (activeCustomers > 0)
            {
                segments.Add(new
                {
                    name = "Active Customers",
                    count = activeCustomers,
                    percentage = Math.Round((activeCustomers / (double)totalCustomers) * 100, 1),
                    color = "#2ecc71"
                });
            }

            if (inactiveCustomers > 0)
            {
                segments.Add(new
                {
                    name = "Inactive Customers",
                    count = inactiveCustomers,
                    percentage = Math.Round((inactiveCustomers / (double)totalCustomers) * 100, 1),
                    color = "#e74c3c"
                });
            }

            return segments;
        }

        public async Task<List<object>> GetTopProductsAsync(string period)
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);

            var topProducts = await customDbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.CreatedAt >= startDate)
                .GroupBy(oi => new {
                    oi.ProductId,
                    oi.Product.Name,
                    oi.Product.Category,
                    oi.Product.ImageUrl
                })
                .Select(g => new
                {
                    id = g.Key.ProductId,
                    name = g.Key.Name,
                    category = g.Key.Category,
                    image = g.Key.ImageUrl,
                    sales = g.Sum(oi => oi.Price * oi.Quantity),
                    units = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(p => p.sales)
                .Take(10)
                .ToListAsync();

            return topProducts.Cast<object>().ToList();
        }

        public async Task<List<object>> GetCategorySalesAsync(string period)
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);
            var previousStartDate = DateTime.UtcNow.AddDays(-days * 2);

            // Current and previous period sales
            var salesData = await GetCategorySalesDataAsync(startDate, previousStartDate);

            var totalRevenue = salesData.Current.Sum(s => s.Revenue);
            var categoryColors = GetCategoryColors();

            var result = salesData.Current.Select(current =>
            {
                var previous = salesData.Previous.FirstOrDefault(p => p.Category == current.Category);
                var previousRevenue = previous?.Revenue ?? 0;
                var growth = previousRevenue > 0 ? ((current.Revenue - previousRevenue) / previousRevenue) * 100 : 0;
                var percentage = totalRevenue > 0 ? (current.Revenue / totalRevenue) * 100 : 0;

                return new
                {
                    name = current.Category,
                    revenue = current.Revenue,
                    percentage = Math.Round(percentage, 1),
                    growth = Math.Round(growth, 1),
                    color = categoryColors.GetValueOrDefault(current.Category, "#95a5a6")
                };
            })
            .OrderByDescending(s => s.revenue)
            .Cast<object>()
            .ToList();

            return result;
        }

        public async Task<List<object>> GetRevenueTrendAsync(string period)
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);

            var orders = await customDbContext.Orders
                .Where(o => o.CreatedAt >= startDate)
                .Select(o => new OrderTrendDto { CreatedAt = o.CreatedAt, TotalAmount = o.TotalAmount })
                .ToListAsync();

            return GenerateTrendData(orders, days);
        }

        public async Task<object> GetOrdersOverviewAsync(string period)
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);

            var orders = await customDbContext.Orders
                .Where(o => o.CreatedAt >= startDate)
                .Select(o => new OrderOverviewDto { Status = o.Status, CreatedAt = o.CreatedAt, TotalAmount = o.TotalAmount })
                .ToListAsync();

            var statusData = orders
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    status = g.Key.ToString(), // Convert enum to string
                    count = g.Count(),
                    percentage = Math.Round((g.Count() / (double)orders.Count) * 100, 1)
                })
                .OrderByDescending(s => s.count)
                .ToList();

            var dailyData = GenerateDailyOrderData(orders, Math.Min(days, 30));

            return new
            {
                statusBreakdown = statusData,
                dailyTrend = dailyData,
                totalOrders = orders.Count,
                totalRevenue = orders.Sum(o => o.TotalAmount)
            };
        }

        // Helper methods
        private static int GetDaysFromPeriod(string period) => period switch
        {
            "7d" => 7,
            "30d" => 30,
            "90d" => 90,
            "1y" => 365,
            _ => 30
        };

        private (decimal TotalRevenue, double RevenueGrowth, int TotalOrders, double OrdersGrowth, decimal AvgOrderValue)
            CalculateMetrics(List<Order> current, List<Order> previous)
        {
            var totalRevenue = current.Sum(o => o.TotalAmount);
            var previousRevenue = previous.Sum(o => o.TotalAmount);
            var revenueGrowth = previousRevenue > 0 ? ((totalRevenue - previousRevenue) / previousRevenue) * 100 : 0;

            var totalOrders = current.Count;
            var previousOrderCount = previous.Count;
            var ordersGrowth = previousOrderCount > 0 ? ((totalOrders - previousOrderCount) / (double)previousOrderCount) * 100 : 0;

            var avgOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            return (
                Math.Round(totalRevenue, 2),
                Math.Round((double)revenueGrowth, 1),
                totalOrders,
                Math.Round(ordersGrowth, 1),
                Math.Round(avgOrderValue, 2)
            );
        }

        private async Task<(int TotalCustomers, double CustomerGrowth, int NewCustomers, double ConversionRate)>
            CalculateCustomerMetricsAsync(DateTime startDate, DateTime previousStartDate)
        {
            var newCustomers = await customDbContext.Users.Where(x => x.Role == "Customer")
                .Where(c => c.CreatedAt >= startDate)
                .CountAsync();

            var totalCustomersInDb = await customDbContext.Users.Where(x => x.Role == "Customer").CountAsync();

            // Simplified conversion rate calculation
            var activeCustomers = await customDbContext.Orders
                .Where(o => o.CreatedAt >= startDate)
                .Select(o => o.UserId)
                .Distinct()
                .CountAsync();

            var conversionRate = totalCustomersInDb > 0 ? (activeCustomers / (double)totalCustomersInDb) * 100 : 0;

            return (activeCustomers, 0, newCustomers, Math.Round(conversionRate, 1));
        }

        private Dictionary<string, string> GetCategoryColors() => new()
    {
        { "Electronics", "#667eea" },
        { "Clothing", "#2ecc71" },
        { "Furniture", "#f39c12" },
        { "Books", "#9b59b6" },
        { "Sports", "#e74c3c" },
        { "Home", "#1abc9c" },
        { "Beauty", "#e67e22" },
        { "Automotive", "#34495e" },
        { "Health", "#16a085" }
    };

        private List<object> GenerateTrendData(List<OrderTrendDto> orders, int days)
        {
            var trendData = new List<object>();

            if (days <= 30)
            {
                // Daily data
                for (int i = days - 1; i >= 0; i--)
                {
                    var date = DateTime.UtcNow.AddDays(-i).Date;
                    var dayOrders = orders.Where(o => o.CreatedAt.Date == date);

                    trendData.Add(new
                    {
                        date = date.ToString("MMM dd"),
                        revenue = dayOrders.Sum(o => o.TotalAmount),
                        orders = dayOrders.Count()
                    });
                }
            }
            else
            {
                // Weekly data
                var weeks = (int)Math.Ceiling(days / 7.0);
                for (int i = weeks - 1; i >= 0; i--)
                {
                    var weekStart = DateTime.UtcNow.AddDays(-i * 7 - 6).Date;
                    var weekEnd = DateTime.UtcNow.AddDays(-i * 7).Date;
                    var weekOrders = orders.Where(o => o.CreatedAt.Date >= weekStart && o.CreatedAt.Date <= weekEnd);

                    trendData.Add(new
                    {
                        date = $"{weekStart:MMM dd} - {weekEnd:MMM dd}",
                        revenue = weekOrders.Sum(o => o.TotalAmount),
                        orders = weekOrders.Count()
                    });
                }
            }

            return trendData;
        }

        private List<object> GenerateDailyOrderData(List<OrderOverviewDto> orders, int days)
        {
            var dailyData = new List<object>();

            for (int i = days - 1; i >= 0; i--)
            {
                var date = DateTime.UtcNow.AddDays(-i).Date;
                var dayOrders = orders.Where(o => o.CreatedAt.Date == date);

                dailyData.Add(new
                {
                    date = date.ToString("MMM dd"),
                    orders = dayOrders.Count(),
                    revenue = dayOrders.Sum(o => o.TotalAmount)
                });
            }

            return dailyData;
        }

        private async Task<(List<CategorySalesDto> Current, List<CategorySalesDto> Previous)>
            GetCategorySalesDataAsync(DateTime startDate, DateTime previousStartDate)
        {
            var current = await customDbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.CreatedAt >= startDate)
                .GroupBy(oi => oi.Product.Category)
                .Select(g => new CategorySalesDto
                {
                    Category = g.Key,
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .ToListAsync();

            var previous = await customDbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.CreatedAt >= previousStartDate && oi.Order.CreatedAt < startDate)
                .GroupBy(oi => oi.Product.Category)
                .Select(g => new CategorySalesDto
                {
                    Category = g.Key,
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .ToListAsync();

            return (current, previous);
        }
    }


}
