using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace DepoYonetimSistemi.Data
{
    class AppDbContext : DbContext
    {
        //DbContext Constructor bağlantı ayarları
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData
            (
                new User
                {
                    UserId = 1,
                    UserName = "Admin",
                    Password = "Admin",
                    Role = UserRole.SystemAdmin,
                    CreatedAt = new DateTime(2025,1,1)
                }
            );
        }
    }
}