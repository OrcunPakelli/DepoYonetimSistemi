using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Models;

namespace DepoYonetimSistemi.Data
{
    class AppDbContext : DbContext
    {
        //DbContext Constructor bağlantı ayarları
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}