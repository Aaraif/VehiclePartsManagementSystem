using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VehiclePartsManagementSystem.Models;
using VehiclePartsManagementSystem.Data;

namespace VehiclePartsManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalParts = _context.Parts.Count();

            ViewBag.TotalSuppliers = _context.Suppliers.Count();

            ViewBag.TotalQuantity = _context.Parts.Sum(p => p.Quantity);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}