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
            /* PARTS */

            ViewBag.TotalParts =
                _context.Parts.Count();

            /* SUPPLIERS */

            ViewBag.TotalSuppliers =
                _context.Suppliers.Count();

            /* TOTAL STOCK */

            ViewBag.TotalQuantity =
                _context.Parts.Sum(p => p.Quantity);

            /* CUSTOMERS */

            ViewBag.TotalCustomers =
                _context.Customers.Count();

            /* TOTAL SALES */

            ViewBag.TotalSales =
                _context.Sales.Count();

            /* TOTAL REVENUE */

            ViewBag.TotalRevenue =
                _context.Sales.Sum(s =>
                    (decimal?)s.FinalAmount) ?? 0;

            /* TODAY REVENUE */

            ViewBag.TodayRevenue =
                _context.Sales
                    .Where(s => s.SaleDate.Date ==
                        DateTime.Today)
                    .Sum(s =>
                        (decimal?)s.FinalAmount) ?? 0;

            /* MONTHLY REVENUE */

            ViewBag.MonthlyRevenue =
                _context.Sales
                    .Where(s =>
                        s.SaleDate.Month ==
                        DateTime.Now.Month &&

                        s.SaleDate.Year ==
                        DateTime.Now.Year)
                    .Sum(s =>
                        (decimal?)s.FinalAmount) ?? 0;

            /* YEARLY REVENUE */

            ViewBag.YearlyRevenue =
                _context.Sales
                    .Where(s =>
                        s.SaleDate.Year ==
                        DateTime.Now.Year)
                    .Sum(s =>
                        (decimal?)s.FinalAmount) ?? 0;

            /* LOW STOCK ITEMS */

            ViewBag.LowStockCount =
                _context.Parts.Count(p => p.Quantity < 10);

            /* TOP CUSTOMER */

            ViewBag.TopCustomer =
                _context.Customers
                    .OrderByDescending(c =>
                        _context.Sales
                            .Where(s => s.CustomerId == c.Id)
                            .Sum(s =>
                                (decimal?)s.FinalAmount) ?? 0)
                    .FirstOrDefault()?.CustomerName
                    ?? "No Customers";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId =
                    Activity.Current?.Id ??
                    HttpContext.TraceIdentifier
            });
        }
    }
}