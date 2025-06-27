using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Orders
{
    public class OrderItemDto
    {
        [Column(TypeName = "uuid")]
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
