using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASOMS.Core.Constants;

namespace ASOMS.DAL.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsConfirmed { get; set; } = false;

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PickupTime { get; set; }

        public string? NotesToBuyer { get; set; }

        public string? NotesToSeller { get; set; }

        // Navigation
        public ICollection<OrderItem> Items { get; set; }
    }

}
