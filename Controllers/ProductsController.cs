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
            // Kullanıcı adı bilgisi ve
            // Rol bilgisi sessiondan alınır 
            // username ve role değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            // Eğer kullanıcı adı boş ise
            if (string.IsNullOrEmpty(username))
            {
                // Kullanıcı Login sayfasına yönlendirilir
                return RedirectToAction("Login", "Home");
            }
            // Eğer kullanıcı rolü SystemAdmin veya Manager değilse
            if (role != UserRole.SystemAdmin.ToString() && role != UserRole.Manager.ToString())
            {
                // Kullanıcı Ana sayfaya yönlendirilir
                return RedirectToAction("Index", "Home");
            }

            // Gpu, Cpu gibi veriler
            // Veri tabanından çekilir
            // ViewBagler ile View'a gönderilir
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
            string serialnum,
            List<int> selectedCpuIds,
            List<int> selectedGpuIds,
            List<int> selectedRamIds,
            List<int> selectedStorageIds,
            List<int> selectedScreenIds)
        {
            // Seri numarası bilgisi boş işe
            if (string.IsNullOrWhiteSpace(serialnum))
            {
                // Hata measjı gönderilir
                ModelState.AddModelError("SeriNumber", "Seri numarası boş olamaz");
            }

            // Girilen seri numarası
            // Veri tabanında aratılır
            // Eğer seri numarası zaten veri tabanında kayıtlı ise
            // Serial değişkeni true değer alır
            bool serial = _context.ProductStocks.Any(sr => sr.SeriNumber == serialnum);

            // Eğer serial değişkeni true ise
            if (serial)
            {
                // Hata measjı gönderilir
                ModelState.AddModelError("", "Bu seri numarası zaten sisteme kayıtlı");
                return View(product);
            }

            // Kullancıdan gelen veriler
            // Uygun ise
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

                // Seçilen depo veritbanında yok ise
                if (warehouse == null)
                {
                    // Hata measjı gönderilir
                    ModelState.AddModelError("", "Depo bulunamadı.");
                    return View(product);
                }

                // Ürün kodu oluşturur
                product.SerialNumber = GenerateProductCode(product, warehouse.WarehouseName, aisle, shelf, bin);

                // Ürünü veritbanına ekle
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
                    SeriNumber = serialnum
                });

                _context.SaveChanges();

                // Raporlama işlemi
                // kullanıcı bilgisi sessiondan alınır
                // Username değişkenine atanır 
                var username = HttpContext.Session.GetString("Username");
                var user = _context.Users.FirstOrDefault(u => u.UserName == username);

                // eğer kullanıcı bilgisi null değil ise
                if (user != null)
                {
                    // Yeni işlem kaydı oluşturulur
                    var log = new Transaction
                    {
                        ProductId = product.ProductId,
                        UserId = user.UserId,
                        TransactionType = TransactionType.In,
                        SeriNo = serialnum,
                        CreatedAt = DateTime.Now
                    };
                    // İşlem kaydı veritabanına eklenir
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

        // Ürün kodu oluşturma metodu
        private string GenerateProductCode(Product product, string warehouse, string aisle, string shelf, string bin)
        {
            // Marka bilgisinden ilk iki harf alınır
            string brandPart = product.Brand.Length >= 2
                ? product.Brand.Substring(0, 2).ToUpper()
                : product.Brand.ToUpper();
            // Model bilgisinden ilk iki harf alınır
            string modelPart = product.Model.Length >= 2
                ? product.Model.Substring(0, 2).ToUpper()
                : product.Model.ToUpper();
            // Depo isminden ilk üç harf alınır
            string warehousePart = warehouse.Length >= 3
                ? warehouse.Substring(0, 3).ToUpper()
                : warehouse.ToUpper();
            // Aisle, Shelf, Bin bilgileri büyük harfe çevrilir
            string aislePart = aisle.ToUpper();
            string shelfPart = shelf.ToUpper();
            string binPart = bin.ToUpper();
            // Tüm parçalar birleştirilir ve ürün kodu oluşturulur
            return $"{brandPart}{modelPart}{warehousePart}{aislePart}{shelfPart}{binPart}";
        }

        // GET: Remove
        [HttpGet]
        public IActionResult Remove()
        {
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi ve
            // Rol bilgisi sessiondan alınır 
            // username ve role değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            // Eğer kullanıcı adı boş ise
            if (string.IsNullOrEmpty(username))
            {
                // Kullanıcı Login sayfasına yönlendirilir
                return RedirectToAction("Login", "Home");
            }
            // Eğer kullanıcı rolü SystemAdmin veya Manager değilse
            if (role != UserRole.SystemAdmin.ToString() && role != UserRole.Manager.ToString())
            {
                // Kullanıcı Ana sayfaya yönlendirilir
                return RedirectToAction("Index", "Home");
            }

            // Tablo gösterimi için ürünler veri tabanından çekilir
            var products = _context.Products
                .Include(p => p.Location)
                .Include(p => p.ProductStocks)
                .ToList();

            return View(products);
        }

        // POST: Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(string serialNumber)
        {


            // Kullanıcının girdiği seri numarası boş ise
            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                // Hata measjı gönderilir
                ModelState.AddModelError("serialNumber", "Lütfen seri numarası girin.");
            }
            // Seri numarasına göre stok kaydını bul
            var stock = _context.ProductStocks
                .Include(s => s.Product)
                    .ThenInclude(p => p.Location)
                .FirstOrDefault(s => s.SeriNumber == serialNumber);
            // Eğer stok kaydı bulunamazsa
            if (stock == null)
                ModelState.AddModelError("serialNumber", "Bu seri numarasına ait ürün bulunamadı.");

            // Model durumu geçerli değil ise
            if (!ModelState.IsValid)
            {
                var list = _context.Products
                    .Include(p => p.Location)
                    .Include(p => p.ProductStocks)
                    .ToList();

                return View(list);
            }

            // İlgili ürün product değişkenine atanır
            var product = stock?.Product;

            // Eğer ürün bulunamazsa
            if (stock == null)
            {
                // Hata measjı gönderilir
                ModelState.AddModelError("", "Bu ürün için stok kaydı bulunamadı.");
                return View();
            }

            // Ürün ve stok kaydı veritabanından silinir
            _context.ProductStocks.Remove(stock);
            _context.Products.Remove(product);

            // Raporlama işlemi
            // kullanıcı bilgisi sessiondan alınır
            // Username değişkenine atanır 
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            // eğer kullanıcı bilgisi null değil ise
            if (user != null)
            {
                // Yeni işlem kaydı oluşturulur
                _context.Transactions.Add(new Transaction
                {
                    ProductId = product.ProductId,
                    UserId = user.UserId,
                    TransactionType = TransactionType.Out,
                    SeriNo = serialNumber,
                    CreatedAt = DateTime.Now
                });
            }

            // Değişikliker veritabanına kaydedilir
            _context.SaveChanges();

            return RedirectToAction("Remove");
        }

    }
}