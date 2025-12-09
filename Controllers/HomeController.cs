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

    public IActionResult Index()
    {
        //Eğer kullanıcı giriş yapmamışsa Index sayfası yerine Login sayfasına yönlendiriliyor
        var username = HttpContext.Session.GetString("Username");//Sessiondan kullanıcı adı bilgisi alınıyor
        if (string.IsNullOrEmpty(username))//Giriş yapılmış mı kontrol ediliyor
        {
            return RedirectToAction("Login", "Home");//Login sayfasına yönlendiriliyor
        }

        var products = _context.Products
            .Include(p => p.Location)
            .Include(p => p.ProductCpus)
                .ThenInclude(pc => pc.Cpu)
            .Include(p => p.ProductGpus)
                .ThenInclude(pg => pg.Gpu)
            .Include(p => p.ProductRams)
                .ThenInclude(pr => pr.Ram)
            .Include(p => p.ProductStorages)
                .ThenInclude(ps => ps.Storage)
            .Include(p => p.ProductScreens)
                .ThenInclude(psc => psc.Screen)
            .Include(p => p.ProductStocks)
                .ThenInclude(ps => ps.Warehouse)
            .ToList();

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
