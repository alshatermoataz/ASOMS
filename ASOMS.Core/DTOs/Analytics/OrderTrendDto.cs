using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Analytics
{
    public class OrderTrendDto
    {
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
