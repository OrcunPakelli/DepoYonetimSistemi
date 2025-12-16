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

        public IActionResult Index()
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

            var logs = _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Product)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return View(logs);
        }
    }
}