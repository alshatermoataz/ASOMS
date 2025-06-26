using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASOMS.DAL.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(20)]
        public string Role { get; set; } = "Customer";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        // --- Profile Info ---
        public string FullName { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // --- Contact Info ---
        public string ContactNumber { get; set; } = string.Empty;
        public string? CurrentAddressLine1 { get; set; }
        public string? Postcode { get; set; }
        public int? CountryLocationId { get; set; }
        public int? NationalityCountryId { get; set; }

         //--- Security ---
        public bool EnableMFA { get; set; }
        public bool IsCreatedByRecruiter { get; set; }
        public bool IsEmailVerified { get; set; }

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
