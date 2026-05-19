using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Data;
using VehiclePartsManagementSystem.Models;

namespace VehiclePartsManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LOGIN

        public IActionResult Login()
        {
            return View();
        }

        // POST: LOGIN

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Customers
                    .FirstOrDefaultAsync(c =>

                        c.Email == model.Email &&

                        c.Password == model.Password);

                if (user != null)
                {
                    /* STORE SESSION */

                    HttpContext.Session.SetString(
                        "UserName",
                        user.CustomerName);

                    HttpContext.Session.SetString(
                        "UserRole",
                        user.Role);

                    HttpContext.Session.SetInt32(
                        "UserId",
                        user.Id);

                    TempData["SuccessMessage"] =
                        "Login successful!";

                    return RedirectToAction(
                        "Index",
                        "Home");
                }

                ModelState.AddModelError("",
                    "Invalid email or password.");
            }

            return View(model);
        }

        // LOGOUT

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] =
                "Logged out successfully!";

            return RedirectToAction(
                "Login",
                "Account");
        }
    }
}