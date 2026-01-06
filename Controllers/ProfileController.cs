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
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi sessiondan alınır 
            // username değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            // Tablo gösterilen kullanıcı bilgileri
            // Veri tabanından alınır
            var users = _context.Users.ToList();

            // Kullanıcı rolü kontrolü
            // Rol bilgisi sessiondan alınır
            // role değişkenine atanır
            var role = HttpContext.Session.GetString("Role");
            // Eğer kullanıcı rolü Sistem Yöneticisi ise
            if (role == UserRole.SystemAdmin.ToString())
            {
                // AdminIndex view'ı döndürülür
                ViewBag.warehouses = _context.WareHouses.ToList();
                return View("AdminIndex", users);
            }
            // Eğer kullanıcı rolü Yönetici ise
            if (role == UserRole.Manager.ToString())
            {
                // Anasayfaya yönlendirilir
                return RedirectToAction("Index", "Home");
            }
            // Eğer kullanıcı rolü Çalışan ise
            if (role == UserRole.Employee.ToString())
            {
                // Anasayfaya yönlendirilir
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminChangePassword(int userId, string newPassword, string confirmPassword)
        {
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi sessiondan alınır 
            // username değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            // Kullanıcı rolü kontrolü
            // Rol bilgisi sessiondan alınır
            // role değişkenine atanır
            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
            {
                return RedirectToAction("Index");
            }

            // Şifre değiştirme işlemi
            // Eğer girilen yeni şifre boş ise
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Şifre boş olmamalı";
                return RedirectToAction("Index");
            }
            // Eğer girilen yeni şifre ile onay şifresi uyuşmuyor ise
            if (newPassword != confirmPassword)
            {
                // Hata Mesajı döndürülür
                TempData["err"] = "Şifreler birbiriyle uyuşmuyor";
            }

            // Kullanıcı veritabanından alınır
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            // Eğer kullanıcı bulunamazsa
            if (user == null)
            {
                // Hata Mesajı döndürülür
                TempData["err"] = "Kullanıcı bulunamadı";
                return RedirectToAction("Index");
            }

            // Yeni şifre kullanıcıya atanır
            user.Password = newPassword;
            // Değişiklikler veritabanına kaydedilir
            _context.SaveChanges();
            // Başarı Mesajı döndürülür
            TempData["Msg"] = $"{user.UserName} için şifre güncellendi";
            return RedirectToAction("Index");
        }

        public IActionResult CreateUser(string userName, string password, string confirmPassword, int userRole)
        {
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi sessiondan alınır 
            // username değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            // Kullanıcı rolü kontrolü
            // Rol bilgisi sessiondan alınır
            // role değişkenine atanır
            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
            {
                return RedirectToAction("Index");
            }

            // Yeni kullanıcı oluşturma işlemi
            // Eğer kullanıcı adı boş ise
            if (string.IsNullOrWhiteSpace(userName))
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Kullanıcı adı boş olamaz.";
                return RedirectToAction("Index");
            }

            // Eğer şifre boş ise
            if (string.IsNullOrWhiteSpace(password))
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Şifre boş olmamalı";
                return RedirectToAction("Index");
            }
            // Eğer şifre ile onay şifresi uyuşmuyor ise
            if (password != confirmPassword)
            {
                // Hata Mesajı döndürülür
                TempData["err"] = "Şifreler birbiriyle uyuşmuyor";
            }

            // Eğer aynı kullanıcı adı zaten var ise
            if (_context.Users.Any(u => u.UserName == userName))
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Bu kullanıcı adı zaten var.";
                return RedirectToAction("Index");
            }

            // Yeni kullanıcı nesnesi oluşturulur
            var newUser = new User
            {
                UserName = userName.Trim(),
                Password = password,
                Role = (UserRole)userRole,
                CreatedAt = DateTime.Now
            };

            // Yeni kullanıcı veritabanına eklenir
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Başarı Mesajı döndürülür
            TempData["Msg"] = "Kullanıcı başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSelectedUsers(string ids)
        {
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi sessiondan alınır 
            // username değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            // Kullanıcı rolü kontrolü
            // Rol bilgisi sessiondan alınır
            // role değişkenine atanır
            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString())
            {
                return RedirectToAction("Index");
            }

            // Kullnacı silme işlemi
            // Eğer ids parametresi boş ise
            if (string.IsNullOrWhiteSpace(ids))
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Silinecek kullanıcı seçilmedi.";
                return RedirectToAction("Index");
            }

            // Ids parametresi virgülle ayrılarak int listesine dönüştürülür
            var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s, out var x) ? x : (int?)null)
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .Distinct()
                    .ToList();

            // Eğer idList boş ise
            if (idList.Count == 0)
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Geçersiz seçim.";
                return RedirectToAction("Index");
            }

            // Kullanıcı kendi hesabını silemesin
            // Mevcut kullanıcı veritabanından alınır ve
            // currentuser değişkenine atanır
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == username);
            // Eğer currentuser değişkeni null değil ise
            if (currentUser != null)
            {
                // Kendi userId'si idList'ten çıkarılır
                idList.Remove(currentUser.UserId);
            }

            // Silinecek kullanıcılar veritabanından alınır
            var usersToDelete = _context.Users.Where(u => idList.Contains(u.UserId)).ToList();

            // Eğer silinecek kullanıcı bulunamazsa
            if (usersToDelete.Count == 0)
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Silinecek kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            // Kullanıcılar veritabanından silinir
            _context.Users.RemoveRange(usersToDelete);
            // Değişiklikler veritabanına kaydedilir
            _context.SaveChanges();
            // Başarı Mesajı döndürülür
            TempData["Msg"] = $"{usersToDelete.Count} kullanıcı silindi.";
            return RedirectToAction("Index");
        }

        public IActionResult AddWareHouse(string warehouse)
        {
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi sessiondan alınır 
            // username değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            // Kullanıcı rolü kontrolü
            // Rol bilgisi sessiondan alınır
            // role değişkenine atanır
            var role = HttpContext.Session.GetString("Role");
            // Eğer kullanıcı rolü Sistem Yöneticisi değilse
            if (role != UserRole.SystemAdmin.ToString())
            {
                // Anasayfaya yönlendirilir
                return RedirectToAction("Index");
            }

            // Depo ekleme işlemi
            // Eğer depo adı boş ise
            if (string.IsNullOrWhiteSpace(warehouse))
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Depo Adı boş olamaz";
                return RedirectToAction("Index");
            }

            // Eğer aynı depo adı zaten var ise
            // exists değişkeni true değer alır
            bool exists = _context.WareHouses.Any(wr => wr.WarehouseName == warehouse);
            // Eğer exists true ise
            if (exists)
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "Bu depo zaten mevcut";
                return RedirectToAction("Index");
            }

            // Yeni depo nesnesi oluşturulur ve veritabanına eklenir
            _context.WareHouses.Add(new WareHouse
            {
                // Depo adı atanır
                WarehouseName = warehouse.Trim(),
            });

            // Değişiklikler veritabanına kaydedilir
            _context.SaveChanges();
            // Başarı Mesajı döndürülür
            TempData["Msg"] = "Depo Eklendi";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCpu(Cpu cpu)
        {
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi sessiondan alınır 
            // username değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Home");

            // Kullanıcı rolü kontrolü
            // Rol bilgisi sessiondan alınır
            // role değişkenine atanır
            var role = HttpContext.Session.GetString("Role");
            // Eğer kullanıcı rolü Sistem Yöneticisi değilse
            if (role != UserRole.SystemAdmin.ToString())
                // Anasayfaya yönlendirilir
                return RedirectToAction("Index");

            // CPU ekleme işlemi
            // Eğer CPU modeli boş ise
            if (string.IsNullOrWhiteSpace(cpu.Model))
            {
                // Hata Mesajı döndürülür
                TempData["Err"] = "CPU modeli boş olamaz";
                return RedirectToAction("Index");
            }
            
            // Yeni CPU veritabanına eklenir
            _context.Cpus.Add(cpu);
            // Değişiklikler veritabanına kaydedilir
            _context.SaveChanges();
            // Başarı Mesajı döndürülür
            TempData["Msg"] = "CPU başarıyla eklendi";
            return RedirectToAction("Index");
        }

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