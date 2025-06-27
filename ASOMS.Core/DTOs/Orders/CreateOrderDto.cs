using ASOMS.Core.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Column(TypeName = "uuid")]
        public Guid UserId { get; set; } // TEMP: Include user manually
        public String PaymentMethod { get; set; } = string.Empty;

        public String NotesToSeller { get; set; } = string.Empty;

        public string PickupTime { get; set; } = string.Empty;

        public List<OrderItemDto> Items { get; set; } = [];
    }

}
