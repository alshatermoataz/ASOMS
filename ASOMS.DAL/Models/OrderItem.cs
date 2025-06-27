using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.DAL.Models
{
    public class OrderItem
    {
        [Key]
        [Column(TypeName = "uuid")]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "uuid")]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        [Column(TypeName = "uuid")]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; } // price per unit
    }
}
