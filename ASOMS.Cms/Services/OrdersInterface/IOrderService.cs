using ASOMS.Core.DTOs.Products;
using ASOMS.DAL.Models;

namespace ASOMS.Cms.Services.OrdersInterface
{
    public interface IOrderService
    {
        Task<byte[]> Generate(List<Order> orders);

    }
}
