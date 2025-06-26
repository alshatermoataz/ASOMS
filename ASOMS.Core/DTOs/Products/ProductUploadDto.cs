using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Products
{
    public class ProductUploadDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Size { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public IFormFile Image { get; set; } = null!;
    }
}
