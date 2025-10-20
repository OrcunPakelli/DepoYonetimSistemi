using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace DepoYonetimSistemi.Data
{
    public class AppDbContext : DbContext
    {
        //DbContext Constructor bağlantı ayarları
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData
            (
                new User
                {
                    UserId = 1,
                    UserName = "admin",
                    Password = "admin",
                    Role = UserRole.SystemAdmin,
                    CreatedAt = new DateTime(2025, 1, 1)
                }
            );
        }
    }
}