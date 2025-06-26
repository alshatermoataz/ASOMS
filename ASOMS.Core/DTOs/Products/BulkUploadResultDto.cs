using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Products
{
    public class BulkUploadResultDto
    {
        public int TotalRecords { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
        public string FileName { get; set; }

        public DateTime ImportDate { get; set; }

        public List<string> Errors { get; set; }
    }
}
