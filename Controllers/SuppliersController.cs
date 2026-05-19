using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Models;
using VehiclePartsManagementSystem.Data;
using VehiclePartsManagementSystem.Helpers;

public class SuppliersController : Controller
{
    private readonly ApplicationDbContext _context;

    public SuppliersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ADMIN AUTHORIZATION CHECK

    private IActionResult? CheckAdminAccess()
    {
        if (!AuthorizationHelper.IsLoggedIn(HttpContext))
        {
            TempData["SuccessMessage"] =
                "Please login first.";

            return RedirectToAction(
                "Login",
                "Account");
        }

        if (!AuthorizationHelper.IsAdmin(HttpContext))
        {
            TempData["SuccessMessage"] =
                "Access denied. Admin only.";

            return RedirectToAction(
                "Index",
                "Home");
        }

        return null;
    }

    // GET: SUPPLIERS

    public async Task<IActionResult> Index()
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        return View(await _context.Suppliers.ToListAsync());
    }

    // GET: SUPPLIERS/Details/5

    public async Task<IActionResult> Details(int? id)
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    // GET: SUPPLIERS/Create

    public IActionResult Create()
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        return View();
    }

    // POST: SUPPLIERS/Create

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(
        [Bind("Id,SupplierName,ContactNumber,Address")]
        Supplier supplier)
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        if (ModelState.IsValid)
        {
            _context.Add(supplier);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Supplier created successfully!";

            return RedirectToAction(nameof(Index));
        }

        return View(supplier);
    }

    // GET: SUPPLIERS/Edit/5

    public async Task<IActionResult> Edit(int? id)
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    // POST: SUPPLIERS/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,SupplierName,ContactNumber,Address")]
        Supplier supplier)
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        if (id != supplier.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(supplier);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(supplier.Id))
                {
                    return NotFound();
                }

                throw;
            }

            TempData["SuccessMessage"] =
                "Supplier updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        return View(supplier);
    }

    // GET: SUPPLIERS/Delete/5

    public async Task<IActionResult> Delete(int? id)
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    // POST: SUPPLIERS/Delete/5

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var accessCheck = CheckAdminAccess();

        if (accessCheck != null)
        {
            return accessCheck;
        }

        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Supplier deleted successfully!";

        return RedirectToAction(nameof(Index));
    }

    // EXISTS CHECK

    private bool SupplierExists(int? id)
    {
        return _context.Suppliers.Any(e => e.Id == id);
    }
}