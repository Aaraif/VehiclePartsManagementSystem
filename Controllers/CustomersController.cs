using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Models;
using VehiclePartsManagementSystem.Data;
using VehiclePartsManagementSystem.Helpers;

public class CustomersController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ADMIN CHECK

    private bool IsAdmin()
    {
        return AuthorizationHelper
            .IsAdmin(HttpContext);
    }

    // CURRENT USER ID

    private int? CurrentUserId()
    {
        return HttpContext.Session
            .GetInt32("UserId");
    }

    // GET: CUSTOMERS

    public async Task<IActionResult> Index(string searchString)
    {
        var customers = _context.Customers.AsQueryable();

        /* SEARCH FILTERS */

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            searchString = searchString.ToLower();

            customers = customers.Where(c =>

                c.CustomerName.ToLower().Contains(searchString) ||

                c.Email.ToLower().Contains(searchString) ||

                c.Phone.ToLower().Contains(searchString) ||

                c.VehicleNumber.ToLower().Contains(searchString) ||

                (c.Address ?? "").ToLower().Contains(searchString)
            );
        }

        return View(await customers.ToListAsync());
    }

    // GET: CUSTOMERS/Details/5

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        /* ONLY ADMIN OR OWNER */

        if (!IsAdmin() &&
            CurrentUserId() != id)
        {
            TempData["SuccessMessage"] =
                "Access denied.";

            return RedirectToAction(
                "Index",
                "Home");
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        /* PURCHASE HISTORY */

        ViewBag.PurchaseHistory = await _context.Sales
            .Include(s => s.Part)
            .Where(s => s.CustomerId == id)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

        return View(customer);
    }

    // GET: CUSTOMERS/Create

    public IActionResult Create()
    {
        return View();
    }

    // POST: CUSTOMERS/Create

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(
        [Bind("Id,CustomerName,Email,Password,Phone,VehicleNumber,Address")]
        Customer customer)
    {
        /* DEFAULT ROLE */

        customer.Role = "Customer";

        /* DUPLICATE EMAIL CHECK */

        bool emailExists = await _context.Customers
            .AnyAsync(c => c.Email == customer.Email);

        if (emailExists)
        {
            ModelState.AddModelError("Email",
                "Email already exists.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(customer);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Customer registered successfully!";

            return RedirectToAction(
                "Login",
                "Account");
        }

        return View(customer);
    }

    // GET: CUSTOMERS/Edit/5

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        /* ONLY ADMIN OR OWNER */

        if (!IsAdmin() &&
            CurrentUserId() != id)
        {
            TempData["SuccessMessage"] =
                "Access denied.";

            return RedirectToAction(
                "Index",
                "Home");
        }

        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        /* ONLY ADMIN CAN EDIT ROLES */

        if (!IsAdmin())
        {
            customer.Role = "Customer";
        }

        return View(customer);
    }

    // POST: CUSTOMERS/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,CustomerName,Email,Password,Phone,VehicleNumber,Address,Role")]
        Customer customer)
    {
        if (id != customer.Id)
        {
            return NotFound();
        }

        /* ONLY ADMIN OR OWNER */

        if (!IsAdmin() &&
            CurrentUserId() != id)
        {
            TempData["SuccessMessage"] =
                "Access denied.";

            return RedirectToAction(
                "Index",
                "Home");
        }

        /* NON-ADMIN CANNOT CHANGE ROLE */

        if (!IsAdmin())
        {
            var existingCustomer =
                await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCustomer != null)
            {
                customer.Role =
                    existingCustomer.Role;
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(customer);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                {
                    return NotFound();
                }

                throw;
            }

            TempData["SuccessMessage"] =
                "Customer updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        return View(customer);
    }

    // GET: CUSTOMERS/Delete/5

    public async Task<IActionResult> Delete(int? id)
    {
        /* ADMIN ONLY */

        if (!IsAdmin())
        {
            TempData["SuccessMessage"] =
                "Only admin can delete users.";

            return RedirectToAction(
                "Index",
                "Home");
        }

        if (id == null)
        {
            return NotFound();
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // POST: CUSTOMERS/Delete/5

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        /* ADMIN ONLY */

        if (!IsAdmin())
        {
            TempData["SuccessMessage"] =
                "Only admin can delete users.";

            return RedirectToAction(
                "Index",
                "Home");
        }

        var customer = await _context.Customers.FindAsync(id);

        if (customer != null)
        {
            _context.Customers.Remove(customer);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Customer deleted successfully!";

        return RedirectToAction(nameof(Index));
    }

    // EXISTS CHECK

    private bool CustomerExists(int? id)
    {
        return _context.Customers.Any(e => e.Id == id);
    }
}