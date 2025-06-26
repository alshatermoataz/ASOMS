using ASOMS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.DAL.EntityFramework
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options)
            : base(options)
        {
        }

        // 👇 Define your tables here
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        // Future: DbSet<Product>, DbSet<Order>, etc.

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Add custom model configuration here
            // e.g., modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
