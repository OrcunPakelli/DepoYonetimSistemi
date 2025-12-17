using Microsoft.AspNetCore.Mvc;
using DepoYonetimSistemi.Models;
using DepoYonetimSistemi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DepoYonetimSistemi.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            // Kullanıcı girişi kontrolü
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            if (role != UserRole.SystemAdmin.ToString() && role != UserRole.Manager.ToString())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Locations = _context.Locations.ToList();
            ViewBag.Cpus = _context.Cpus.ToList();
            ViewBag.Gpus = _context.Gpus.ToList();
            ViewBag.Rams = _context.Rams.ToList();
            ViewBag.Storages = _context.Storages.ToList();
            ViewBag.Screens = _context.Screens.ToList();
            ViewBag.WareHouses = _context.WareHouses.ToList();

            ViewBag.Aisles = _context.Locations
                .Select(l => l.Aisle)
                .Distinct()
                .ToList();

            ViewBag.Shelves = _context.Locations
                .Select(l => l.Shelf)
                .Distinct()
                .ToList();

            ViewBag.Bins = _context.Locations
                .Select(l => l.Bin)
                .Distinct()
                .ToList();
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product,
            string aisle,
            string shelf,
            string bin,
            int warehouseId,
            int stock,
            List<int> selectedCpuIds,
            List<int> selectedGpuIds,
            List<int> selectedRamIds,
            List<int> selectedStorageIds,
            List<int> selectedScreenIds)
        {
            if (ModelState.IsValid)
            {
                // Yeni Location oluştur
                var location = new Location
                {
                    Aisle = aisle,
                    Shelf = shelf,
                    Bin = bin
                };

                _context.Locations.Add(location);
                _context.SaveChanges();

                // Ürüne LocationId ata
                product.LocationId = location.LocationId;

                var warehouse = _context.WareHouses.FirstOrDefault(w => w.WarehouseId == warehouseId);

                if (warehouse == null)
                {
                    ModelState.AddModelError("", "Depo bulunamadı.");
                    return View(product);
                }

                // Seri no oluştur
                product.SerialNumber = GenerateProductCode(product, warehouse.WarehouseName, aisle, shelf, bin);

                // Ürün kaydet
                _context.Products.Add(product);
                _context.SaveChanges();

                // CPU
                if (selectedCpuIds != null)
                {
                    foreach (var cpuId in selectedCpuIds)
                    {
                        _context.ProductCpus.Add(new ProductCpu
                        {
                            ProductId = product.ProductId,
                            CpuId = cpuId
                        });
                    }
                }

                // GPU
                if (selectedGpuIds != null)
                {
                    foreach (var gpuId in selectedGpuIds)
                    {
                        _context.ProductGpus.Add(new ProductGpu
                        {
                            ProductId = product.ProductId,
                            GpuId = gpuId
                        });
                    }
                }

                // RAM
                if (selectedRamIds != null)
                {
                    foreach (var ramId in selectedRamIds)
                    {
                        _context.ProductRams.Add(new ProductRam
                        {
                            ProductId = product.ProductId,
                            RamId = ramId
                        });
                    }
                }

                // Storage
                if (selectedStorageIds != null)
                {
                    foreach (var storageId in selectedStorageIds)
                    {
                        _context.ProductStorages.Add(new ProductStorage
                        {
                            ProductId = product.ProductId,
                            StorageId = storageId
                        });
                    }
                }

                // Screen
                if (selectedScreenIds != null)
                {
                    foreach (var screenId in selectedScreenIds)
                    {
                        _context.ProductScreens.Add(new ProductScreen
                        {
                            ProductId = product.ProductId,
                            ScreenId = screenId
                        });
                    }
                }

                // Stok
                _context.ProductStocks.Add(new ProductStock
                {
                    ProductId = product.ProductId,
                    WarehouseId = warehouseId,
                    StockQuantity = stock
                });

                _context.SaveChanges();

                // Log yaz (User transaction)
                var username = HttpContext.Session.GetString("Username");
                var user = _context.Users.FirstOrDefault(u => u.UserName == username);

                if (user != null)
                {
                    var log = new Transaction
                    {
                        ProductId = product.ProductId,
                        UserId = user.UserId,
                        TransactionType = TransactionType.In,
                        Quantity = stock,
                        CreatedAt = DateTime.Now
                    };
                    _context.Transactions.Add(log);
                    _context.SaveChanges();
                }

                return RedirectToAction("Create", "Products");
            }

            // ViewBagler
            ViewBag.Warehouses = _context.WareHouses.ToList();
            ViewBag.Cpus = _context.Cpus.ToList();
            ViewBag.Gpus = _context.Gpus.ToList();
            ViewBag.Rams = _context.Rams.ToList();
            ViewBag.Storages = _context.Storages.ToList();
            ViewBag.Screens = _context.Screens.ToList();

            ViewBag.Aisles = _context.Locations
                .Select(l => l.Aisle)
                .Distinct()
                .ToList();
            ViewBag.Shelves = _context.Locations
                .Select(l => l.Shelf)
                .Distinct()
                .ToList();
            ViewBag.Bins = _context.Locations
                .Select(l => l.Bin)
                .Distinct()
                .ToList();

            return View(product);
        }

        private string GenerateProductCode(Product product, string warehouse, string aisle, string shelf, string bin)
        {
            string brandPart = product.Brand.Length >= 2
        ? product.Brand.Substring(0, 2).ToUpper()
        : product.Brand.ToUpper();

            string modelPart = product.Model.Length >= 2
                ? product.Model.Substring(0, 2).ToUpper()
                : product.Model.ToUpper();

            string warehousePart = warehouse.Length >= 3
                ? warehouse.Substring(0, 3).ToUpper()
                : warehouse.ToUpper();

            string aislePart = aisle.ToUpper();
            string shelfPart = shelf.ToUpper();
            string binPart = bin.ToUpper();

            return $"{brandPart}{modelPart}{warehousePart}{aislePart}{shelfPart}{binPart}";
        }

        // GET: Remove
        [HttpGet]
        public IActionResult Remove()
        {
            // Kullanıcı girişi kontrolü
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != UserRole.SystemAdmin.ToString() && role != UserRole.Manager.ToString())
            {
                return RedirectToAction("Index", "Home");
            }
            var products = _context.Products
                .Include(p => p.Location)
                .Include(p => p.ProductStocks)
                .Where(p => p.ProductStocks.Any(ps => ps.StockQuantity > 0))
                .ToList();

            return View(products);
        }

        // POST: Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(string serialNumber, int quantity)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                ModelState.AddModelError("serialNumber", "Lütfen seri numarası girin.");

            if (quantity <= 0)
                ModelState.AddModelError("quantity", "Kaldırılacak miktar 0'dan büyük olmalıdır.");

            var product = _context.Products
                .Include(p => p.ProductStocks)
                .FirstOrDefault(p => p.SerialNumber == serialNumber);

            if (product == null)
                ModelState.AddModelError("serialNumber", "Bu seri numarasına ait ürün bulunamadı.");

            ProductStock? stock = null;
            if (product != null)
            {
                stock = _context.ProductStocks
                    .FirstOrDefault(s => s.ProductId == product.ProductId);

                if (stock == null)
                    ModelState.AddModelError("quantity", "Bu ürün için stok kaydı bulunamadı.");
            }

            if (!ModelState.IsValid)
            {
                var list = _context.Products
                    .Include(p => p.Location)
                    .Include(p => p.ProductStocks)
                    .ToList();

                return View(list);
            }

            // Stok kontrolü
            if (stock!.StockQuantity < quantity)
            {
                ModelState.AddModelError("quantity", "Stokta yeterli miktar yok.");
                var list = _context.Products
                    .Include(p => p.Location)
                    .Include(p => p.ProductStocks)
                    .ToList();

                return View(list);
            }

            // Stoktan düş
            stock.StockQuantity -= quantity;
            _context.ProductStocks.Update(stock);

            // Log kaydı
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                _context.Transactions.Add(new Transaction
                {
                    ProductId = product!.ProductId,
                    UserId = user.UserId,
                    TransactionType = TransactionType.Out,
                    Quantity = quantity,
                    CreatedAt = DateTime.Now
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Remove", "Products");
        }

    }
}