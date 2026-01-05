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
                ViewBag.warehouses = _context.WareHouses.ToList();
                return View("AdminIndex", users);
            }
            if (role == UserRole.Manager.ToString())
            {
                return RedirectToAction("Index", "Home");
            }
            if (role == UserRole.Employee.ToString())
            {
                return RedirectToAction("Index", "Home");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSelectedUsers(string ids)
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

            if (string.IsNullOrWhiteSpace(ids))
            {
                TempData["Err"] = "Silinecek kullanıcı seçilmedi.";
                return RedirectToAction("Index");
            }

            var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s, out var x) ? x : (int?)null)
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .Distinct()
                    .ToList();

            if (idList.Count == 0)
            {
                TempData["Err"] = "Geçersiz seçim.";
                return RedirectToAction("Index");
            }

            //Kullanıcı kendi hesabını silemesin
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (currentUser != null)
            {
                idList.Remove(currentUser.UserId);
            }

            var usersToDelete = _context.Users.Where(u => idList.Contains(u.UserId)).ToList();

            if (usersToDelete.Count == 0)
            {
                TempData["Err"] = "Silinecek kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            _context.Users.RemoveRange(usersToDelete);
            _context.SaveChanges();

            TempData["Msg"] = $"{usersToDelete.Count} kullanıcı silindi.";
            return RedirectToAction("Index");
        }

        public IActionResult AddWareHouse(string warehouse)
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

            if (string.IsNullOrWhiteSpace(warehouse))
            {
                TempData["Err"] = "Depo Adı boş olamaz";
                return RedirectToAction("Index");
            }

            bool exists = _context.WareHouses.Any(wr => wr.WarehouseName == warehouse);

            if (exists)
            {
                TempData["Err"] = "Bu depo zaten mevcut";
                return RedirectToAction("Index");
            }

            _context.WareHouses.Add(new WareHouse
            {
                WarehouseName = warehouse.Trim(),
            });

            _context.SaveChanges();
            TempData["Msg"] = "Depo Eklendi";
            return RedirectToAction("Index");
        }

        // ================= CPU =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCpu(Cpu cpu)
        {
            // Login kontrol
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Home");

            // Admin kontrol
            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(cpu.Model))
            {
                TempData["Err"] = "CPU modeli boş olamaz";
                return RedirectToAction("Index");
            }

            _context.Cpus.Add(cpu);
            _context.SaveChanges();

            TempData["Msg"] = "CPU başarıyla eklendi";
            return RedirectToAction("Index");
        }

        // ================= GPU =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGpu(Gpu gpu)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Home");

            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(gpu.Model))
            {
                TempData["Err"] = "GPU modeli boş olamaz";
                return RedirectToAction("Index");
            }

            _context.Gpus.Add(gpu);
            _context.SaveChanges();

            TempData["Msg"] = "GPU başarıyla eklendi";
            return RedirectToAction("Index");
        }

        // ================= RAM =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRam(Ram ram)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Home");

            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(ram.Model))
            {
                TempData["Err"] = "RAM modeli boş olamaz";
                return RedirectToAction("Index");
            }

            _context.Rams.Add(ram);
            _context.SaveChanges();

            TempData["Msg"] = "RAM başarıyla eklendi";
            return RedirectToAction("Index");
        }

        // ================= STORAGE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStorage(Storage storage)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Home");

            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(storage.Model))
            {
                TempData["Err"] = "Storage modeli boş olamaz";
                return RedirectToAction("Index");
            }

            _context.Storages.Add(storage);
            _context.SaveChanges();

            TempData["Msg"] = "Storage başarıyla eklendi";
            return RedirectToAction("Index");
        }

        // ================= SCREEN =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddScreen(Screen screen)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Home");

            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(screen.Resolution))
            {
                TempData["Err"] = "Ekran çözünürlüğü boş olamaz";
                return RedirectToAction("Index");
            }

            _context.Screens.Add(screen);
            _context.SaveChanges();

            TempData["Msg"] = "Ekran başarıyla eklendi";
            return RedirectToAction("Index");
        }

    }
}