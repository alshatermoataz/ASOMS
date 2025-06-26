using System.ComponentModel.DataAnnotations;
using ASOMS.Core.Constants;

namespace ASOMS.Core.DTOs.Auth
{
    public class RegisterRequest
    {

        public required string FullName { get; set; }
        public required string Email { get; set; }

        public required string Password { get; set; }
        public required string ContactNumber { get; set; }


        [RegularExpression("Customer|RetailSalesperson|WarehouseManager|Admin",
            ErrorMessage = "Invalid role. Allowed roles: Customer, RetailSalesperson, WarehouseManager, Admin")]
        public string Role { get; set; } = UserRoles.Customer;
    }
}
