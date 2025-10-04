using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DepoYonetimSistemi.Models;
using DepoYonetimSistemi.Data;
using Microsoft.EntityFrameworkCore;

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
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Index(string username, string Password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);//veritabanından kullanıcı bulunuyor

        if (user == null || user.Password != Password)//kullanıcı adı ve şifre kontrol ediliyor
        {
            ViewBag.Message = "Kullanici adi veya sifre hatali";
            return View();
        }

        //session başlatılıyor
        HttpContext.Session.SetString("UserName", user.UserName);
        HttpContext.Session.SetString("Role", user.Role.ToString());

        switch (user.Role) //kullanıcı adı veya şifre yanlış değilse kullanıcı rolü kontrol ediliyor
        {
            case UserRole.SystemAdmin:
                return RedirectToAction("Index", "Admin");
            case UserRole.Manager:
                return RedirectToAction("Index", "Manager");
            case UserRole.Employee:
                return RedirectToAction("Index", "Employee");
            default:
                return RedirectToAction("Index", "Home");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
