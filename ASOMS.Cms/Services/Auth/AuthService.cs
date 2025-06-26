using ASOMS.Core.DTOs.Auth;
using ASOMS.Core.Interfaces;
using ASOMS.DAL.EntityFramework;
using ASOMS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ASOMS.Cms.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly CustomDbContext customDbContext;
        private readonly IConfiguration configuration;

        public AuthService(CustomDbContext db, IConfiguration config)
        {
            customDbContext = db;
            configuration = config;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            if (await customDbContext.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("User with this email already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FullName = request.FullName,
                ContactNumber = request.ContactNumber,
                Role = request.Role,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsEmailVerified = false
            };

            customDbContext.Users.Add(user);
            await customDbContext.SaveChangesAsync();

            return new RegisterResponse
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
                Role = user.Role,
                Token = GenerateJwtToken(user)
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await customDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = GenerateJwtToken(user); // Assuming your JWT generator is working

            return new LoginResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    FullName = user.FullName,
                    ContactNumber = user.ContactNumber,
                    ProfilePictureUrl = user.ProfilePictureUrl ?? string.Empty
 
                }
            };
        }


        private string GenerateJwtToken(User user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role) // ✅ Add this line
        }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var inputHash = HashPassword(inputPassword);
            return storedHash == inputHash;
        }
    }
}
