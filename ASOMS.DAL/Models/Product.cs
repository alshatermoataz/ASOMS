using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASOMS.DAL.Models
{
    public class Product
    {
        [Key]
        [Column(TypeName = "uuid")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public string? Brand { get; set; } // e.g., Zetum, Dongah

        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // e.g., Tyres, Accessories, Oil

        [MaxLength(50)]
        public string? Size { get; set; } // e.g., "175/70/13" or "N50"

        public string? Specifications { get; set; } // Any additional details (JSON or plain text)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}