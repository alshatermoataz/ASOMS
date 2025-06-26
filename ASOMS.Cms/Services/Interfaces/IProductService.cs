using ASOMS.Core.DTOs.Products;
using ASOMS.DAL.Models;

namespace ASOMS.Cms.Services.Interfaces
{
    public interface IProductService
    {
        Task<BulkUploadResultDto> ProcessBulkUpload(IFormFile file);
        Task<Product> UpdateProductDetailsAsync(Guid id, ProductUpdateDto productDto);

        Task<PagedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters);
    }
}
