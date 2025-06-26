using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Orders
{
    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string? NotesToBuyer { get; set; }
    }
}
