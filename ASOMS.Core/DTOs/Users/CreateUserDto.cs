using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Users
{
    public class CreateUserDto
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!; 

        public string? Gender { get; set; } 
        public DateTime? DateOfBirth { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string ContactNumber { get; set; } = null!;
        public string? CurrentAddressLine1 { get; set; }
        public string? Postcode { get; set; }

        public string Role { get; set; } = "Customer";

    }
}
