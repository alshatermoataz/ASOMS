using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.Constants
{
    public enum OrderStatus
    {
        [Display (Name = "Pending Approval")]
        PendingApproval,
        Confirmed,
        Rejected,
        Completed
    }
}
