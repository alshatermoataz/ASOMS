using ASOMS.Cms.Services.Extensions;
using ASOMS.Cms.Services.OrdersInterface;
using ASOMS.Core.Constants;
using ASOMS.Core.DTOs.Orders;
using ASOMS.DAL.EntityFramework;
using ASOMS.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using ASOMS.Cms.Services;

namespace ASOMS.Cms.Controllers.Orders
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(CustomDbContext customDbContext, IOrderService orderService, IHubContext<NotificationHub> hubContext) : ControllerBase
    {

        [HttpPost]
        // [Authorize] ← Remove for now
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            OrderStatus status;
            bool isConfirmed;

            //var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (dto.UserId == Guid.Empty)
                return BadRequest("User ID is required.");

            if (dto.Items == null || !dto.Items.Any())
                return BadRequest("Order must contain at least one item.");

            // Attempt to parse the payment method from string to enum
            if (!Enum.TryParse<PaymentMethod>(dto.PaymentMethod, true, out var paymentMethodEnum))
            {
                return BadRequest("Invalid payment method.");
            }

            if (!DateTime.TryParse(dto.PickupTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsedPickupTime))
            {
                return BadRequest("Invalid pickup time format.");
            }

            // Just to be 100% sure
            parsedPickupTime = DateTime.SpecifyKind(parsedPickupTime, DateTimeKind.Utc);


            switch (paymentMethodEnum)
            {
                case PaymentMethod.Cash:
                    status = OrderStatus.Confirmed;
                    isConfirmed = true;
                    break;
                case PaymentMethod.Invoice:
                    status = OrderStatus.PendingApproval;
                    isConfirmed = false;
                    break;
                default:
                    return BadRequest("Unsupported payment method.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                PaymentMethod = paymentMethodEnum,
                Status = status,
                IsConfirmed = isConfirmed,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0,
                NotesToSeller = dto.NotesToSeller,
                PickupTime = parsedPickupTime,
                Items = new List<OrderItem>()
            };



            foreach (var item in dto.Items)
            {
                var product = await customDbContext.Products.FindAsync(item.ProductId);
                if (product == null) return NotFound($"Product with ID {item.ProductId} not found.");

                if (product.Quantity < item.Quantity)
                    return BadRequest($"Insufficient stock for product {product.Name}.");

                var itemTotal = product.Price * item.Quantity;
                order.TotalAmount += itemTotal;

                product.Quantity -= item.Quantity;

                order.Items.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price,

                });
            }

            customDbContext.Orders.Add(order);
            await customDbContext.SaveChangesAsync();

            // Broadcast order created event
            await hubContext.Clients.All.SendAsync("OrderUpdated", new { order.Id, order.TotalAmount, order.Status });

            return Ok(new { order.Id, order.TotalAmount, order.Status });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(Guid userId)
        {
            var orders = await customDbContext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var result = orders.Select(o => new
            {
                o.Id,
                Status = o.Status.GetDisplayName(), // Convert enum to readable string
                PaymentMethod = o.PaymentMethod.GetDisplayName(), // Convert enum to readable string
                o.TotalAmount,
                o.CreatedAt,
                o.PickupTime,
                o.NotesToBuyer,
                Items = o.Items.Select(i => new
                {
                    i.ProductId,
                    ProductName = i.Product.Name,
                    ProductImage = i.Product.ImageUrl,
                    i.Quantity,
                    i.Price,

                    Total = i.Quantity * i.Price
                })
            });

            return Ok(result);
        }




        [HttpGet]
        public async Task<IActionResult> GetOrders(
        int page = 1,
        int pageSize = 10,
        string? status = null,
        string? paymentMethod = null,
        string? search = null,
        string? sort = null,
        string? dateRange = null,
        decimal? minAmount = null,
        decimal? maxAmount = null)

        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 10;

            var query = customDbContext.Orders.Include(o => o.User).AsQueryable();

            // Parse enums outside EF expression to avoid conversion errors
            OrderStatus? parsedStatus = null;
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse(status, true, out OrderStatus s))
                parsedStatus = s;

            if (parsedStatus.HasValue)
                query = query.Where(o => o.Status == parsedStatus.Value);

            PaymentMethod? parsedPayment = null;
            if (!string.IsNullOrWhiteSpace(paymentMethod) && Enum.TryParse(paymentMethod, true, out PaymentMethod p))
                parsedPayment = p;

            if (parsedPayment.HasValue)
                query = query.Where(o => o.PaymentMethod == parsedPayment.Value);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(o =>
                    o.User.FullName.ToLower().Contains(search) ||
                    o.Id.ToString().Substring(0, 8).ToLower().Contains(search)
                );
            }

            if (!string.IsNullOrWhiteSpace(dateRange))
            {
                DateTime now = DateTime.UtcNow;
                DateTime startDate = dateRange.ToLower() switch
                {
                    "today" => now.Date,
                    "week" => now.Date.AddDays(-(int)now.DayOfWeek),
                    "month" => new DateTime(now.Year, now.Month, 1),
                    "quarter" => new DateTime(now.Year, ((now.Month - 1) / 3) * 3 + 1, 1),
                    _ => DateTime.MinValue
                };

                if (startDate > DateTime.MinValue)
                {
                    query = query.Where(o => o.CreatedAt >= startDate);
                }
            }

            if (minAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= maxAmount.Value);
            }


            query = sort switch
            {
                "date-asc" => query.OrderBy(o => o.CreatedAt),
                "amount-desc" => query.OrderByDescending(o => o.TotalAmount),
                "amount-asc" => query.OrderBy(o => o.TotalAmount),
                "status" => query.OrderBy(o => o.Status),
                _ => query.OrderByDescending(o => o.CreatedAt) // default: newest first
            };


            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new
                {
                    o.Id,
                    o.UserId,
                    UserFullName = o.User.FullName,
                    PaymentMethod = o.PaymentMethod.GetDisplayName(),
                    Status = o.Status.GetDisplayName(),
                    o.CreatedAt,
                    o.TotalAmount,
                    Items = o.Items.Select(i => new
                    {
                        i.Id,
                        i.Quantity,
                        i.Price,
                        i.ProductId,
                        Product = new
                        {
                            i.Product.Id,
                            i.Product.Name
                        }
                    })
                })
                .ToListAsync();

            return Ok(new
            {
                data = orders,
                totalPages,
                currentPage = page,
                totalItems
            });
        }

        [HttpGet("statuses")]
        public IActionResult GetOrderStatuses()
        {
            var statuses = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(s => new
                {
                    key = s.ToString(),
                    label = string.Concat(
                        s.ToString().Select((ch, i) =>
                            i > 0 && char.IsUpper(ch) ? " " + ch : ch.ToString()
                        )
                    )
                });

            return Ok(statuses);
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await customDbContext.Orders.FindAsync(orderId);
            if (order == null) return NotFound("Order not found");

            if (!Enum.TryParse<OrderStatus>(dto.Status, true, out var parsedStatus))
                return BadRequest("Invalid order status.");

            order.Status = parsedStatus;

            if (!string.IsNullOrWhiteSpace(dto.NotesToBuyer))
                order.NotesToBuyer = dto.NotesToBuyer;

            // Optional: You can automatically mark it confirmed if new status is 'Confirmed'
            order.IsConfirmed = parsedStatus == OrderStatus.Confirmed;

            await customDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Order status updated successfully.",
                orderId = order.Id,
                newStatus = order.Status.ToString()
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await customDbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound(new { message = "Order not found." });

            // Remove associated items first if needed
            customDbContext.OrderItems.RemoveRange(order.Items);
            customDbContext.Orders.Remove(order);

            await customDbContext.SaveChangesAsync();

            return Ok(new { message = "Order deleted successfully." });
        }

        [HttpPost("download-invoices")]
        public async Task<IActionResult> DownloadInvoices([FromBody] List<Guid> orderIds)
        {
            try
            {
                if (orderIds == null || !orderIds.Any())
                    return BadRequest(new { message = "No order IDs provided." });

                var orders = await customDbContext.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .Where(o => orderIds.Contains(o.Id))
                    .ToListAsync();

                if (orders.Count == 0)
                    return NotFound(new { message = "No orders found." });

                var pdfBytes = await orderService.Generate(orders);
                return File(pdfBytes, "application/pdf", "Invoices.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Download invoice failed: " + ex.Message);
                return StatusCode(500, new { message = "Server error", detail = ex.Message });
            }
        }





    }



}
