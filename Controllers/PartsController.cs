using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Models;
using VehiclePartsManagementSystem.Data;

public class PartsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PartsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /* AUTHORIZATION CHECK */

    private bool IsStaffOrAdmin()
    {
        var role =
            HttpContext.Session.GetString("UserRole");

        return role == "Admin" ||
               role == "Staff";
    }

    /* ACCESS DENIED */

    private IActionResult AccessDenied()
    {
        TempData["SuccessMessage"] =
            "Access denied! Staff or Admin only.";

        return RedirectToAction(
            "Index",
            "Home");
    }

    // GET: PARTS

    public async Task<IActionResult> Index(string searchString)
    {
        var parts = _context.Parts
            .Include(p => p.Supplier)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            parts = parts.Where(s =>
                s.PartName.Contains(searchString));
        }

        return View(await parts.ToListAsync());
    }

    // GET: PARTS/Details/5

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var part = await _context.Parts
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (part == null)
        {
            return NotFound();
        }

        return View(part);
    }

    // GET: PARTS/Create

    public IActionResult Create()
    {
        if (!IsStaffOrAdmin())
        {
            return AccessDenied();
        }

        ViewBag.SupplierId =
            new SelectList(_context.Suppliers,
            "Id",
            "SupplierName");

        return View();
    }

    // POST: PARTS/Create

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(
        [Bind("Id,PartName,Quantity,Price,SupplierId")] Part part)
    {
        if (!IsStaffOrAdmin())
        {
            return AccessDenied();
        }

        if (ModelState.IsValid)
        {
            _context.Add(part);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Part created successfully!";

            return RedirectToAction(nameof(Index));
        }

        ViewBag.SupplierId =
            new SelectList(_context.Suppliers,
            "Id",
            "SupplierName",
            part.SupplierId);

        return View(part);
    }

    // GET: PARTS/Edit/5

    public async Task<IActionResult> Edit(int? id)
    {
        if (!IsStaffOrAdmin())
        {
            return AccessDenied();
        }

        if (id == null)
        {
            return NotFound();
        }

        var part = await _context.Parts.FindAsync(id);

        if (part == null)
        {
            return NotFound();
        }

        ViewBag.SupplierId =
            new SelectList(_context.Suppliers,
            "Id",
            "SupplierName",
            part.SupplierId);

        return View(part);
    }

    // POST: PARTS/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,PartName,Quantity,Price,SupplierId")] Part part)
    {
        if (!IsStaffOrAdmin())
        {
            return AccessDenied();
        }

        if (id != part.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(part);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(part.Id))
                {
                    return NotFound();
                }

                throw;
            }

            TempData["SuccessMessage"] =
                "Part updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        ViewBag.SupplierId =
            new SelectList(_context.Suppliers,
            "Id",
            "SupplierName",
            part.SupplierId);

        return View(part);
    }

    // GET: PARTS/Delete/5

    public async Task<IActionResult> Delete(int? id)
    {
        if (!IsStaffOrAdmin())
        {
            return AccessDenied();
        }

        if (id == null)
        {
            return NotFound();
        }

        var part = await _context.Parts
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (part == null)
        {
            return NotFound();
        }

        return View(part);
    }

    // POST: PARTS/Delete/5

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (!IsStaffOrAdmin())
        {
            return AccessDenied();
        }

        var part = await _context.Parts.FindAsync(id);

        if (part != null)
        {
            _context.Parts.Remove(part);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Part deleted successfully!";

        return RedirectToAction(nameof(Index));
    }

    // EXISTS CHECK

    private bool PartExists(int? id)
    {
        return _context.Parts.Any(e => e.Id == id);
    }
}