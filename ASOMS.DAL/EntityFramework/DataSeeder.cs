using ASOMS.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ASOMS.DAL.EntityFramework
{
    public static class DataSeeder
    {
        public static void Seed(CustomDbContext context)
        {
            context.Database.Migrate();

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "admin@asoms.com",
                        PasswordHash = Hash("admin123"),
                        FullName = "Admin User",
                        Role = "Admin",
                        ContactNumber = "0111111111",
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "warehouse@asoms.com",
                        PasswordHash = Hash("warehouse123"),
                        FullName = "Warehouse Manager",
                        Role = "WarehouseManager",
                        ContactNumber = "0222222222",
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "retail@asoms.com",
                        PasswordHash = Hash("retail123"),
                        FullName = "Retail Staff",
                        Role = "RetailSalesperson",
                        ContactNumber = "0333333333",
                        IsEmailVerified = true
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            SeedProducts(context);
        }

        public static void SeedProducts(CustomDbContext context)
        {
            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product {
                    Id = Guid.NewGuid(),
                    Name = "Zetum Tyres",
                    Description = "Car tyre 175/70/13",
                    Brand = "Zetum",
                    Category = "Tyres",
                    Size = "175/70/13",
                    Price = 530,
                    Quantity = 20,
                    ImageUrl = "https://cdn.asoms.com/products/zetum-tyres.png"
                },
                new Product {
                    Id = Guid.NewGuid(),
                    Name = "Dongah Battery",
                    Description = "N50 model battery",
                    Brand = "Dongah",
                    Category = "Accessories",
                    Size = "N50",
                    Price = 350,
                    Quantity = 10,
                    ImageUrl = "https://cdn.asoms.com/products/dongah-battery.png"
                },
                new Product {
                    Id = Guid.NewGuid(),
                    Name = "Motor Oil 5W-30",
                    Description = "Synthetic oil for performance engines",
                    Brand = "ASOIL",
                    Category = "Oil",
                    Price = 80,
                    Quantity = 50,
                    ImageUrl = "https://cdn.asoms.com/products/motor-oil-5w30.png"
                },
                new Product {
                    Id = Guid.NewGuid(),
                    Name = "Shell Helix Ultra 5W-40",
                    Description = "Fully synthetic motor oil",
                    Brand = "Shell",
                    Category = "Oil",
                    Price = 90,
                    Quantity = 40,
                    ImageUrl = "https://cdn.asoms.com/products/shell-helix.png"
                },
                new Product {
                    Id = Guid.NewGuid(),
                    Name = "Mobil 1 5W-30",
                    Description = "Advanced full synthetic oil",
                    Brand = "Mobil",
                    Category = "Oil",
                    Price = 100,
                    Quantity = 35,
                    ImageUrl = "https://cdn.asoms.com/products/mobil1.png"
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static string Hash(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
