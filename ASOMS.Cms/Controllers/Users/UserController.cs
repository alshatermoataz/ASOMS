using ASOMS.Cms.Services.Auth;
using ASOMS.Core.DTOs.Users;
using ASOMS.Core.Interfaces;
using ASOMS.DAL.EntityFramework;
using ASOMS.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using ASOMS.Cms.Services;

namespace ASOMS.Cms.Controllers.Users;

//[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController(CustomDbContext customDbContext, IHubContext<NotificationHub> hubContext) : ControllerBase
{


    [HttpPatch("{id}/role")]
    public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] UserRoleUpdateRequest request)
    {
        var user = await customDbContext.Users.FindAsync(id);
        if (user == null) return NotFound("User not found.");

        user.Role = request.Role;
        await customDbContext.SaveChangesAsync();

        return Ok(new { message = "User role updated successfully.", newRole = user.Role });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(
    int page = 1,
    int pageSize = 10,
    string? status = null,
    string? search = null,
    string? registered = null, // today, week, month, year
    int? minOrders = null,
    string? role = "Customer",
    int? maxOrders = null)
    {
        var query = customDbContext.Users
            .Where(x => x.Role == role)
            .Include(u => u.Orders)
            .AsQueryable();

        // 🔍 Search
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(u =>
                u.FullName.Contains(search) ||
                u.Email.Contains(search));
        }

        // 🟢 Status filter
        if (!string.IsNullOrEmpty(status))
        {
            DateTime thresholdDate = DateTime.UtcNow.AddDays(-30);
            if (status.ToLower() == "active")
            {
                query = query.Where(u => u.Orders.Any(o => o.CreatedAt >= thresholdDate));
            }
            else if (status.ToLower() == "inactive")
            {
                query = query.Where(u => u.Orders.All(o => o.CreatedAt < thresholdDate) || !u.Orders.Any());
            }
        }

        // 📅 Registration filter
        if (!string.IsNullOrEmpty(registered))
        {
            var now = DateTime.UtcNow;

            switch (registered.ToLower())
            {
                case "today":
                    var startOfDay = now.Date;
                    var endOfDay = startOfDay.AddDays(1);
                    query = query.Where(u => u.CreatedAt >= startOfDay && u.CreatedAt < endOfDay);
                    break;

                case "week":
                    var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
                    startOfWeek = DateTime.SpecifyKind(startOfWeek, DateTimeKind.Utc);
                    var endOfWeek = startOfWeek.AddDays(7);
                    query = query.Where(u => u.CreatedAt >= startOfWeek && u.CreatedAt < endOfWeek);
                    break;

                case "month":
                    var startOfMonth = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1), DateTimeKind.Utc);
                    var endOfMonth = startOfMonth.AddMonths(1);
                    query = query.Where(u => u.CreatedAt >= startOfMonth && u.CreatedAt < endOfMonth);
                    break;

                case "year":
                    var startOfYear = DateTime.SpecifyKind(new DateTime(now.Year, 1, 1), DateTimeKind.Utc);
                    var endOfYear = startOfYear.AddYears(1);
                    query = query.Where(u => u.CreatedAt >= startOfYear && u.CreatedAt < endOfYear);
                    break;
            }
        }

        // 🔢 Filter by order count
        if (minOrders.HasValue)
        {
            query = query.Where(u => u.Orders.Count >= minOrders.Value);
        }
        if (maxOrders.HasValue)
        {
            query = query.Where(u => u.Orders.Count <= maxOrders.Value);
        }

        var totalUsers = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.Role,
                u.ContactNumber,
                u.IsEmailVerified,
                u.CreatedAt,
                u.ProfilePictureUrl,
                u.Gender,
                u.DateOfBirth,
                u.CurrentAddressLine1,
                u.Postcode,
                u.LastLogin,
                OrderCount = u.Orders.Count,
                RecentOrders = u.Orders.OrderByDescending(o => o.CreatedAt).Take(10),
                LastOrderDate = u.Orders
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => (DateTime?)o.CreatedAt)
            .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(new
        {
            data = users,
            totalUsers,
            totalPages,
            currentPage = page
        });
    }


    [HttpPost("{id}/upload-profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var user = await customDbContext.Users.FindAsync(id);
        if (user == null)
            return NotFound("User not found.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Update user record
        user.ProfilePictureUrl = $"/uploads/{fileName}";
        await customDbContext.SaveChangesAsync();

        return Ok(new { url = user.ProfilePictureUrl });
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await customDbContext.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict("Email already exists.");

        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        var hashedPassword = Convert.ToBase64String(hashedBytes);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = hashedPassword,
            FullName = dto.FullName,
            ContactNumber = dto.ContactNumber,
            // Assign optional fields only if provided
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth.HasValue
                ? DateTime.SpecifyKind(dto.DateOfBirth.Value, DateTimeKind.Utc)
                : null,
            ProfilePictureUrl = dto.ProfilePictureUrl,
            CurrentAddressLine1 = dto.CurrentAddressLine1,
            Postcode = dto.Postcode,
            Role = dto.Role
        };

        customDbContext.Users.Add(user);
        await customDbContext.SaveChangesAsync();

        await hubContext.Clients.All.SendAsync("CustomerUpdated", new { user.Id, user.FullName, user.Email });

        return Ok(new { message = "Customer created successfully", user.Id });
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await customDbContext.Users.FindAsync(id);
        if (user == null)
            return NotFound("User not found.");

        // Required fields
        user.FullName = dto.FullName;
        user.Email = dto.Email;

        user.ContactNumber = dto.ContactNumber;

        // Optional fields
        if (!string.IsNullOrWhiteSpace(dto.Gender))
            user.Gender = dto.Gender;

        if (dto.DateOfBirth.HasValue)
            user.DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth.Value, DateTimeKind.Utc);

        if (!string.IsNullOrWhiteSpace(dto.ProfilePictureUrl))
            user.ProfilePictureUrl = dto.ProfilePictureUrl;

        if (!string.IsNullOrWhiteSpace(dto.CurrentAddressLine1))
            user.CurrentAddressLine1 = dto.CurrentAddressLine1;

        if (!string.IsNullOrWhiteSpace(dto.Postcode))
            user.Postcode = dto.Postcode;

        await customDbContext.SaveChangesAsync();

        // Broadcast customer updated event
        await hubContext.Clients.All.SendAsync("CustomerUpdated", new
        {
            user.Id,
            user.FullName,
            user.Email
        });

        return Ok(new { message = "User updated successfully", user.Id });
    }



}

