using ASOMS.DAL.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASOMS.Cms.Controllers.Analytics
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController(CustomDbContext customDbContext) : ControllerBase
    {

        [HttpGet("dashboard-summary")]
        public async Task<IActionResult> GetDashboardSummary([FromQuery] string period = "30d")
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);
            var previousStartDate = DateTime.UtcNow.AddDays(-days * 2);

            // Current period data
            var currentOrders = await customDbContext.Orders
                .Where(o => o.CreatedAt >= startDate)
                .ToListAsync();

            // Previous period data for comparison
            var previousOrders = await customDbContext.Orders
                .Where(o => o.CreatedAt >= previousStartDate && o.CreatedAt < startDate)
                .ToListAsync();

            var totalRevenue = currentOrders.Sum(o => o.TotalAmount);
            var previousRevenue = previousOrders.Sum(o => o.TotalAmount);
            var revenueGrowth = previousRevenue > 0 ? ((totalRevenue - previousRevenue) / previousRevenue) * 100 : 0;

            var totalOrders = currentOrders.Count;
            var previousOrderCount = previousOrders.Count;
            var ordersGrowth = previousOrderCount > 0 ? ((totalOrders - previousOrderCount) / (double)previousOrderCount) * 100 : 0;

            var avgOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            // Customer metrics
            var customerIds = currentOrders.Select(o => o.UserId).Distinct().ToList();
            var totalCustomers = customerIds.Count;

            var newCustomers = await customDbContext.Users
                .Where(c => c.Role == "Customer" && c.CreatedAt >= startDate)
                .CountAsync();

            var previousCustomerIds = previousOrders.Select(o => o.UserId).Distinct().ToList();
            var previousCustomerCount = previousCustomerIds.Count;
            var customerGrowth = previousCustomerCount > 0 ? ((totalCustomers - previousCustomerCount) / (double)previousCustomerCount) * 100 : 0;

            // Conversion rate (simplified - orders vs total customers)
            var totalCustomersInDb = await customDbContext.Users.Where(x => x.Role == "Customer").CountAsync();
            var conversionRate = totalCustomersInDb > 0 ? (totalCustomers / (double)totalCustomersInDb) * 100 : 0;

            return Ok(new
            {
                totalRevenue,
                revenueGrowth = Math.Round(revenueGrowth, 1),
                totalOrders,
                ordersGrowth = Math.Round(ordersGrowth, 1),
                avgOrderValue = Math.Round(avgOrderValue, 2),
                totalCustomers,
                customerGrowth = Math.Round(customerGrowth, 1),
                newCustomers,
                conversionRate = Math.Round(conversionRate, 1),
                period
            });
        }

        [HttpGet("customer-segments")]
        public async Task<IActionResult> GetCustomerSegments()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            // Get all customers with their order activity
            var customers = await customDbContext.Users.Where(x=> x.Role == "Customer")
                .Select(c => new
                {
                    c.Id,
                    c.CreatedAt,
                    HasRecentOrders = customDbContext.Orders.Any(o => o.UserId == c.Id && o.CreatedAt >= thirtyDaysAgo)
                })
                .ToListAsync();

            var totalCustomers = customers.Count;
            var activeCustomers = customers.Count(c => c.HasRecentOrders);
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

            return Ok(segments);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] string period = "30d")
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);

            var topProducts = await customDbContext.OrderItems
                .Where(oi => oi.Order.CreatedAt >= startDate)
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name, oi.Product.Category, oi.Product.ImageUrl })
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
                .Take(5)
                .ToListAsync();

            return Ok(topProducts);
        }

        [HttpGet("category-sales")]
        public async Task<IActionResult> GetCategorySales([FromQuery] string period = "30d")
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);
            var previousStartDate = DateTime.UtcNow.AddDays(-days * 2);

            // Current period sales by category
            var currentSales = await customDbContext.OrderItems
                .Where(oi => oi.Order.CreatedAt >= startDate)
                .GroupBy(oi => oi.Product.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .ToListAsync();

            // Previous period sales for growth calculation
            var previousSales = await customDbContext.OrderItems
                .Where(oi => oi.Order.CreatedAt >= previousStartDate && oi.Order.CreatedAt < startDate)
                .GroupBy(oi => oi.Product.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .ToDictionaryAsync(x => x.Category, x => x.Revenue);

            var totalRevenue = currentSales.Sum(s => s.Revenue);

            var categoryColors = new Dictionary<string, string>
        {
            { "Tyres", "#667eea" },
            { "Accessories", "#2ecc71" },
            { "Oil", "#f39c12" },
            { "Parts", "#9b59b6" },
            { "Batteries", "#e74c3c" },
        };

            var result = currentSales.Select(s =>
            {
                var previousRevenue = previousSales.GetValueOrDefault(s.Category, 0);
                var growth = previousRevenue > 0 ? ((s.Revenue - previousRevenue) / previousRevenue) * 100 : 0;
                var percentage = totalRevenue > 0 ? (s.Revenue / totalRevenue) * 100 : 0;

                return new
                {
                    name = s.Category,
                    revenue = s.Revenue,
                    percentage = Math.Round(percentage, 1),
                    growth = Math.Round(growth, 1),
                    color = categoryColors.GetValueOrDefault(s.Category, "#95a5a6")
                };
            })
            .OrderByDescending(s => s.revenue)
            .ToList();

            return Ok(result);
        }

        [HttpGet("revenue-trend")]
        public async Task<IActionResult> GetRevenueTrend([FromQuery] string period = "30d")
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);

            var orders = await customDbContext.Orders
                .Where(o => o.CreatedAt >= startDate)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync();

            var trendData = new List<object>();

            if (days <= 7)
            {
                // Daily data for 7 days or less
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
            else if (days <= 30)
            {
                // Daily data for 30 days
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
                // Weekly data for longer periods
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

            return Ok(trendData);
        }

        [HttpGet("orders-overview")]
        public async Task<IActionResult> GetOrdersOverview([FromQuery] string period = "30d")
        {
            var days = GetDaysFromPeriod(period);
            var startDate = DateTime.UtcNow.AddDays(-days);

            var orders = await customDbContext.Orders
                .Where(o => o.CreatedAt >= startDate)
                .ToListAsync();

            // Group by status
            var statusData = orders
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    status = g.Key,
                    count = g.Count(),
                    percentage = Math.Round((g.Count() / (double)orders.Count) * 100, 1)
                })
                .OrderByDescending(s => s.count)
                .ToList();

            // Daily order counts
            var dailyData = new List<object>();
            for (int i = Math.Min(days, 30) - 1; i >= 0; i--)
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

            return Ok(new
            {
                statusBreakdown = statusData,
                dailyTrend = dailyData,
                totalOrders = orders.Count,
                totalRevenue = orders.Sum(o => o.TotalAmount)
            });
        }

        private static int GetDaysFromPeriod(string period)
        {
            return period switch
            {
                "7d" => 7,
                "30d" => 30,
                "90d" => 90,
                "1y" => 365,
                _ => 30
            };
        }
    }
}
