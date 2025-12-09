using DepoYonetimSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var logs = _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Product)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return View(logs);
        }
    }
}