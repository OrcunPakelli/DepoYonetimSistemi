using DepoYonetimSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Models;

namespace DepoYonetimSistemi.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(List<int> userIds, List<string> types, DateTime? startDate, DateTime? endDate)
        {
            // Kullanıcı girişi kontrolü
            // Kullanıcı adı bilgisi sessiondan alınır 
            // username değişkenine atanır
            var username = HttpContext.Session.GetString("Username");
            // Eğer kullanıcı adı boş ise
            if (string.IsNullOrEmpty(username))
            {
                // Login sayfasına yönlendirilir
                return RedirectToAction("Login", "Home");
            }
            // Kullanıcı rolü kontrolü
            // Rol bilgisi sessiondan alınır
            // role değişkenine atanır
            var role = HttpContext.Session.GetString("Role");
            // Eğer kullanıcı rolü Sistem Yöneticisi veya Yönetici değilse
            if (role != UserRole.SystemAdmin.ToString() && role != UserRole.Manager.ToString())
            {
                // Anasayfaya yönlendirilir
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Users = _context.Users.OrderBy(u => u.UserName).ToList();

            // Filtrelenmiş işlem kayıtlarının alınması
            var query = _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Product)
                .AsQueryable();

            // Filtreleme koşulları
            // Eğer userIds listesi boş değil ise
            if (userIds != null && userIds.Count > 0)
                // İlgili kullanıcıların işlemleri filtrelenir
                query = query.Where(t => userIds.Contains(t.UserId));
            // Eğer types listesi boş değil ise
            if (types != null && types.Count > 0)
                // İlgili işlem türlerinin kayıtları filtrelenir
                query = query.Where(t => types.Contains(t.TransactionType.ToString()));
            // Eğer startDate değeri var ise
            if (startDate.HasValue)
                // Başlangıç tarihine göre filtreleme yapılır
                query = query.Where(t => t.CreatedAt >= startDate.Value);
            // Eğer endDate değeri var ise
            if (endDate.HasValue)
                // Bitiş tarihine göre filtreleme yapılır
                query = query.Where(t => t.CreatedAt <= endDate.Value.AddDays(1).AddTicks(-1));
            // Filtrelenmiş kayıtlar tarihe göre sıralanır ve listeye dönüştürülür
            var logs = query
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            // Dropdownlar
            ViewBag.Users = _context.Users.OrderBy(u => u.UserName).ToList();
            ViewBag.Products = _context.Products.OrderBy(p => p.Brand).ThenBy(p => p.Model).ToList();

            // Filtrelenmiş işlem kayıtları view'a gönderilir
            return View(logs);
        }
    }
}
