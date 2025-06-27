using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Auth
{
    public class UserDto
    {
        [Column(TypeName = "uuid")]
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? ContactNumber { get; set; } = string.Empty;


        public string? ProfilePictureUrl { get; set; } = string.Empty;
    }
}
