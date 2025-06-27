using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Products
{
    // Models/ProductDto.cs - Make sure nullable properties are properly defined
    public class ProductDto
    {
        [Column(TypeName = "uuid")]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Brand { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Size { get; set; }
        public string? Specifications { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Computed properties for status
        public string Status { get; set; } = "Inactive";
        public DateTime? LastOrderDate { get; set; } // This should be nullable
        public int OrderCountLast30Days { get; set; } = 0;
        public decimal RevenueLast30Days { get; set; } = 0;
    }
}
