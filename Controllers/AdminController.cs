using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Data;

namespace VehiclePartsManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ADMIN PANEL

        public async Task<IActionResult> Index()
        {
            /* SESSION CHECK */

            var userRole =
                HttpContext.Session.GetString("UserRole");

            if (userRole != "Admin")
            {
                return Content(
                    "ACCESS DENIED : ADMIN ONLY");
            }

            /* LOAD USERS */

            var users = await _context.Customers
                .ToListAsync();

            return View(users);
        }
    }
}