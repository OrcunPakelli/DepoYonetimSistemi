using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimSistemi.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            //Login kontrol
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            //Kullanıcıları al
            var users = _context.Users.ToList();

            var role = HttpContext.Session.GetString("Role");
            //Rol kontrolü
            if (role == UserRole.SystemAdmin.ToString())
            {
                return View("AdminIndex", users);
            }
            if (role == UserRole.Manager.ToString())
            {
                return View("ManagerIndex");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminChangePassword(int userId, string newPassword, string confirmPassword)
        {
            //Login kontrol
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            //Admin kontrol
            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
            {
                return RedirectToAction("Index");
            }

            //Şifre Deiştirme
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                TempData["Err"] = "Şifre boş olmamalı";
                return RedirectToAction("Index");
            }
            if (newPassword != confirmPassword)
            {
                TempData["err"] = "Şifreler birbiriyle uyuşmuyor";
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                TempData["err"] = "Kullanıcı bulunamadı";
                return RedirectToAction("Index");
            }

            user.Password = newPassword;
            _context.SaveChanges();
            TempData["Msg"] = $"{user.UserName} için şifre güncellendi";
            return RedirectToAction("Index");
        }

        public IActionResult CreateUser(string userName, string password, string confirmPassword, int userRole)
        {
            //Login kontrol
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            //Admin kontrol
            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                TempData["Err"] = "Kullanıcı adı boş olamaz.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                TempData["Err"] = "Şifre boş olmamalı";
                return RedirectToAction("Index");
            }
            if (password != confirmPassword)
            {
                TempData["err"] = "Şifreler birbiriyle uyuşmuyor";
            }

            if (_context.Users.Any(u => u.UserName == userName))
            {
                TempData["Err"] = "Bu kullanıcı adı zaten var.";
                return RedirectToAction("Index");
            }

            var newUser = new User
            {
                UserName = userName.Trim(),
                Password = password,
                Role = (UserRole)userRole,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            TempData["Msg"] = "Kullanıcı başarıyla eklendi.";
            return RedirectToAction("Index");
        }
    }
}