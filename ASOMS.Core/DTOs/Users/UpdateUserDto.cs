using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Users
{
    public class UpdateUserDto
    {
        [Required]
        public string FullName { get; set; }

        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? CurrentAddressLine1 { get; set; }
        public string? Postcode { get; set; }
        public string? ContactNumber { get; set; }

    }

}
