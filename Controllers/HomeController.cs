using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DepoYonetimSistemi.Models;
using DepoYonetimSistemi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace DepoYonetimSistemi.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index(
    string? brand,
    string? model,
    string? serialNumber,
    int? locationId,
    List<int>? cpuId,
    List<int>? gpuId,
    List<int>? ramSize,
    List<string>? ramType,
    List<int>? storageCapacity,
    List<string>? storageType,
    List<double>? screenSize,
    List<string>? screenResolution,
    List<int>? warehouseId
    )
    {
        // Kullanıcı girişi kontrolü
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Home");
        }

        // --- DROPDOWN VERİLERİ ---

        // Lokasyon (koridor/raf/kutu)
        ViewBag.Locations = _context.Locations.ToList();

        // CPU / GPU listeleri
        ViewBag.Cpus = _context.Cpus.ToList();
        ViewBag.Gpus = _context.Gpus.ToList();

        // RAM boyutları ve tipleri
        ViewBag.RamSizes = _context.Rams
            .Select(r => r.SizeGB)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        ViewBag.RamTypes = _context.Rams
            .Select(r => r.Type)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        // Depolama kapasiteleri ve tipleri
        ViewBag.StorageCapacities = _context.Storages
            .Select(s => s.CapacityGB)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        ViewBag.StorageTypes = _context.Storages
            .Select(s => s.Type)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        // Ekran boyutları ve çözünürlükleri
        ViewBag.ScreenSizes = _context.Screens
            .Select(s => s.SizeInches)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        ViewBag.ScreenResolutions = _context.Screens
            .Select(s => s.Resolution)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        // Depolar
        ViewBag.Warehouses = _context.WareHouses.ToList();

        // --- ÜRÜN SORGUSU ---

        var query = _context.Products
            .Include(p => p.Location)
            .Include(p => p.ProductCpus).ThenInclude(pc => pc.Cpu)
            .Include(p => p.ProductGpus).ThenInclude(pg => pg.Gpu)
            .Include(p => p.ProductRams).ThenInclude(pr => pr.Ram)
            .Include(p => p.ProductStorages).ThenInclude(ps => ps.Storage)
            .Include(p => p.ProductScreens).ThenInclude(psc => psc.Screen)
            .Include(p => p.ProductStocks).ThenInclude(ps => ps.Warehouse)
            .AsQueryable();

        // Marka
        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(p => p.Brand.Contains(brand));

        // Model
        if (!string.IsNullOrWhiteSpace(model))
            query = query.Where(p => p.Model.Contains(model));

        // Seri No
        // Seri No (ProductStocks üzerinden)
        if (!string.IsNullOrWhiteSpace(serialNumber))
        {
            query = query.Where(p =>
                p.ProductStocks.Any(ps => ps.SeriNumber.Contains(serialNumber))
            );
        }
        // Lokasyon
        if (locationId.HasValue)
            query = query.Where(p => p.LocationId == locationId.Value);

        // --- TEKNİK ÖZELLİK FİLTRELERİ ---

        // CPU
        if (cpuId != null && cpuId.Any())
        {
            query = query.Where(p =>
                p.ProductCpus.Any(pc => cpuId.Contains(pc.CpuId)));
        }
        // GPU
        if (gpuId != null && gpuId.Any())
        {
            query = query.Where(p =>
                p.ProductGpus.Any(pg => gpuId.Contains(pg.GpuId)));
        }

        // RAM boyutu
        if (ramSize != null && ramSize.Any())
        {
            query = query.Where(p =>
                p.ProductRams.Any(pr => ramSize.Contains(pr.Ram.SizeGB)));
        }

        // RAM tipi
        if (ramType != null && ramType.Any())
        {
            query = query.Where(p =>
                p.ProductRams.Any(pr => ramType.Contains(pr.Ram.Type)));
        }
        // Depolama kapasitesif
        if (storageCapacity != null && storageCapacity.Any())
        {
            query = query.Where(p =>
                p.ProductStorages.Any(ps => storageCapacity.Contains(ps.Storage.CapacityGB)));
        }

        // Depolama tipi
        if (storageType != null && storageType.Any())
        {
            query = query.Where(p =>
                p.ProductStorages.Any(ps => storageType.Contains(ps.Storage.Type)));
        }


        // Ekran boyutu
        if (screenSize != null && screenSize.Any())
        {
            query = query.Where(p =>
                p.ProductScreens.Any(psc => screenSize.Contains(psc.Screen.SizeInches)));
        }

        // Ekran çözünürlüğü
        if (screenResolution != null && screenResolution.Any())
        {
            query = query.Where(p =>
                p.ProductScreens.Any(psc => screenResolution.Contains(psc.Screen.Resolution)));
        }

        // Depo (warehouse) – ürünün stoğu hangi depoda ise
        if (warehouseId != null && warehouseId.Any())
        {
            query = query.Where(p =>
                p.ProductStocks.Any(ps => warehouseId.Contains(ps.WarehouseId)));
        }

        var products = query.ToList();

        return View(products);
    }

    public IActionResult Login()
    {
        //Eğer kullanıcı giriş yapmışsa Login sayfası yerine Index sayfasına yönlendiriliyor
        var username = HttpContext.Session.GetString("Username");//Sessiondan kullanıcı adı bilgisi alınıyor
        if (!string.IsNullOrEmpty(username))//Giriş yapılmış mı kontrol ediliyor
        {
            return RedirectToAction("Index", "Home");//Index sayfasına yönlendiriliyor
        }
        return View();
    }


    [HttpPost]
    public IActionResult Login(string Username, string Password)
    {
        //kullanıcı giriş işlemler
        var user = _context.Users.FirstOrDefault(u => u.UserName == Username && u.Password == Password);//Girilen giriş bilgileri kontrol ediliyor
        if (user != null)//Kullanıcı veri tabanına kayıtlı mı kontrol ediliyor
        {
            HttpContext.Session.SetString("Username", user.UserName);
            HttpContext.Session.SetString("Role", user.Role.ToString());
            return RedirectToAction("Index", "Home");
        }

        ViewBag.ErrorMessage = "kullanıcı adı veya şifre hatalı";//Yanlış girişde hatalı mesajı gösterir
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();//Session sonlandırılıyor

        return RedirectToAction("Login", "Home");//Login safyasına yönlendiriliyor
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
