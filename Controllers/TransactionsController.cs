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
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }
            var role = HttpContext.Session.GetString("Role");
            if (role != UserRole.SystemAdmin.ToString() && role != UserRole.Manager.ToString())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Users = _context.Users.OrderBy(u => u.UserName).ToList();

            var query = _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Product)
                .AsQueryable();

            if (userIds != null && userIds.Count > 0)
                query = query.Where(t => userIds.Contains(t.UserId));

            if (types != null && types.Count > 0)
                query = query.Where(t => types.Contains(t.TransactionType.ToString()));

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CreatedAt <= endDate.Value.AddDays(1).AddTicks(-1));

            var logs = query
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            // Dropdownlar
            ViewBag.Users = _context.Users.OrderBy(u => u.UserName).ToList();
            ViewBag.Products = _context.Products.OrderBy(p => p.Brand).ThenBy(p => p.Model).ToList();

            return View(logs);
        }
    }
}
