using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Products
{
    public class ProductQueryParameters
    {
        public string? Category { get; set; } = string.Empty;
        public string? Search { get; set; } = string.Empty; 
        public string? StockLevel { get; set; } = string.Empty; 
        public string? Status { get; set; } = string.Empty;
        public int? MinStock { get; set; } 
        public int? MaxStock { get; set; }
        public string? SortBy { get; set; } = "name";
        public string? SortDirection { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
