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

        //Ana Tablolar
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<Location> Locations { get; set; }


        //Donanım Tabloları
        public DbSet<Cpu> Cpus { get; set; }
        public DbSet<Gpu> Gpus { get; set; }
        public DbSet<Ram> Rams { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Screen> Screens { get; set; }

        //Ara Tablolar
        public DbSet<ProductCpu> ProductCpus { get; set; }
        public DbSet<ProductGpu> ProductGpus { get; set; }
        public DbSet<ProductRam> ProductRams { get; set; }
        public DbSet<ProductStorage> ProductStorages { get; set; }
        public DbSet<ProductScreen> ProductScreens { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductStock>()
               .ToTable(tb => tb.HasTrigger("trg_DeleteProductWhenAllStockZero"));

            // Product - Warehouse many-to-many
            modelBuilder.Entity<ProductStock>()
                .HasKey(ps => new { ps.ProductId, ps.WarehouseId });
            modelBuilder.Entity<ProductStock>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductStocks)
                .HasForeignKey(ps => ps.ProductId);
            modelBuilder.Entity<ProductStock>()
                .HasOne(ps => ps.Warehouse)
                .WithMany(w => w.ProductStocks)
                .HasForeignKey(ps => ps.WarehouseId);

            //Product - Cpu many-to-many
            modelBuilder.Entity<ProductCpu>()
                .HasKey(pc => new { pc.ProductId, pc.CpuId });
            modelBuilder.Entity<ProductCpu>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCpus)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductCpu>()
                .HasOne(pc => pc.Cpu)
                .WithMany(c => c.ProductCpus)
                .HasForeignKey(pc => pc.CpuId);

            //Product - Gpu many-to-many
            modelBuilder.Entity<ProductGpu>()
                .HasKey(pg => new { pg.ProductId, pg.GpuId });
            modelBuilder.Entity<ProductGpu>()
                .HasOne(pg => pg.Product)
                .WithMany(p => p.ProductGpus)
                .HasForeignKey(pg => pg.ProductId);
            modelBuilder.Entity<ProductGpu>()
                .HasOne(pg => pg.Gpu)
                .WithMany(g => g.ProductGpus)
                .HasForeignKey(pg => pg.GpuId);

            //Product - Ram many-to-many
            modelBuilder.Entity<ProductRam>()
                .HasKey(pr => new { pr.ProductId, pr.RamId });
            modelBuilder.Entity<ProductRam>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.ProductRams)
                .HasForeignKey(pr => pr.ProductId);
            modelBuilder.Entity<ProductRam>()
                .HasOne(pr => pr.Ram)
                .WithMany(r => r.ProductRams)
                .HasForeignKey(pr => pr.RamId);

            //Product - Storage  many-to-many
            modelBuilder.Entity<ProductStorage>()
                .HasKey(ps => new { ps.ProductId, ps.StorageId });
            modelBuilder.Entity<ProductStorage>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductStorages)
                .HasForeignKey(ps => ps.ProductId);
            modelBuilder.Entity<ProductStorage>()
                .HasOne(ps => ps.Storage)
                .WithMany(s => s.ProductStorages)
                .HasForeignKey(ps => ps.StorageId);

            //Product - Screen  many-to-many
            modelBuilder.Entity<ProductScreen>()
                .HasKey(psc => new { psc.ProductId, psc.ScreenId });
            modelBuilder.Entity<ProductScreen>()
                .HasOne(psc => psc.Product)
                .WithMany(p => p.ProductScreens)
                .HasForeignKey(psc => psc.ProductId);
            modelBuilder.Entity<ProductScreen>()
                .HasOne(psc => psc.Screen)
                .WithMany(s => s.ProductScreens)
                .HasForeignKey(psc => psc.ScreenId);

            //Product - Location one-to-many
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Location)
                .WithMany(l => l.Products)
                .HasForeignKey(p => p.LocationId);

            //User  - Transaction  one-to-many
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            //Product - Transaction one-to-many
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Product)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.ProductId);



            //Seed Data
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

            modelBuilder.Entity<Cpu>().HasData
            (
                    new Cpu { CpuId = 1, Model = "Core i5-12400F", Cores = 6, Threads = 12, BaseClockGHz = 2.5, BoostClockGHz = 4.4, Manufacturer = "Intel" },
                    new Cpu { CpuId = 2, Model = "Ryzen 5 5600X", Cores = 6, Threads = 12, BaseClockGHz = 3.7, BoostClockGHz = 4.6, Manufacturer = "AMD" }
            );

            modelBuilder.Entity<Gpu>().HasData
            (
                    new Gpu { GpuId = 1, Model = "RTX 3060", Memory = "12GB GDDR6", Manufacturer = "NVIDIA" },
                    new Gpu { GpuId = 2, Model = "RX 6600 XT", Memory = "8GB GDDR6", Manufacturer = "AMD" }
            );

            modelBuilder.Entity<Ram>().HasData
            (
                    new Ram { RamId = 1, Model = "Vengeance LPX", Type = "DDR4", SizeGB = 16, SpeedMHz = 3200, Manufacturer = "Corsair" },
                    new Ram { RamId = 2, Model = "Fury Beast", Type = "DDR4", SizeGB = 32, SpeedMHz = 3600, Manufacturer = "Kingston" }
            );

            modelBuilder.Entity<Storage>().HasData
            (
                    new Storage { StorageId = 1, Model = "970 EVO Plus", Type = "NVMe SSD", CapacityGB = 1000, Manufacturer = "Samsung" },
                    new Storage { StorageId = 2, Model = "Barracuda", Type = "HDD", CapacityGB = 1000, Manufacturer = "Seagate" }
            );

            modelBuilder.Entity<Screen>().HasData(
                    new Screen { ScreenId = 1, Resolution = "3840x2160", SizeInches = 27, PanelType = "IPS", RefreshRate = 60 },
                    new Screen { ScreenId = 2, Resolution = "1920x1080", SizeInches = 24, PanelType = "IPS", RefreshRate = 165 }
            );
            modelBuilder.Entity<Location>().HasData(
                    new Location { LocationId = 1, Aisle = "A", Shelf = "1", Bin = "01" },
                    new Location { LocationId = 2, Aisle = "A", Shelf = "2", Bin = "05" },
                    new Location { LocationId = 3, Aisle = "B", Shelf = "1", Bin = "02" },
                    new Location { LocationId = 4, Aisle = "C", Shelf = "3", Bin = "04" }
            );
        }
    }
}