using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Models;
using VehiclePartsManagementSystem.Data;

public class SalesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SalesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SALES

    public async Task<IActionResult> Index()
    {
        var sales = _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Part);

        return View(await sales.ToListAsync());
    }

    // GET: SALES/Details/5

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sale = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Part)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (sale == null)
        {
            return NotFound();
        }

        return View(sale);
    }

    // GET: SALES/Create

    public async Task<IActionResult> Create(int? partId)
    {
        ViewBag.CustomerId =
            new SelectList(_context.Customers,
            "Id",
            "CustomerName");

        ViewBag.PartId =
            new SelectList(_context.Parts,
            "Id",
            "PartName");

        var sale = new Sale();

        /* AUTO SELECT PART */

        if (partId != null)
        {
            sale.PartId = partId.Value;

            var selectedPart = await _context.Parts
                .FirstOrDefaultAsync(p => p.Id == partId);

            ViewBag.SelectedPart = selectedPart;
        }

        /* AUTO SELECT CUSTOMER */

        var userId =
            HttpContext.Session.GetInt32("UserId");

        if (userId != null)
        {
            sale.CustomerId = userId.Value;
        }

        return View(sale);
    }

    // POST: SALES/Create

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(
        [Bind("Id,CustomerId,PartId,Quantity")] Sale sale)
    {
        if (ModelState.IsValid)
        {
            var part = await _context.Parts
                .FirstOrDefaultAsync(p => p.Id == sale.PartId);

            if (part == null)
            {
                return NotFound();
            }

            /* LOW STOCK VALIDATION */

            if (sale.Quantity > part.Quantity)
            {
                ModelState.AddModelError("",
                    "Not enough stock available.");

                ViewBag.CustomerId =
                    new SelectList(_context.Customers,
                    "Id",
                    "CustomerName",
                    sale.CustomerId);

                ViewBag.PartId =
                    new SelectList(_context.Parts,
                    "Id",
                    "PartName",
                    sale.PartId);

                return View(sale);
            }

            /* AUTO TOTAL CALCULATION */

            sale.TotalAmount =
                part.Price * sale.Quantity;

            /* LOYALTY DISCOUNT */

            if (sale.TotalAmount > 5000)
            {
                sale.DiscountAmount =
                    sale.TotalAmount * 0.10m;
            }
            else
            {
                sale.DiscountAmount = 0;
            }

            /* FINAL TOTAL */

            sale.FinalAmount =
                sale.TotalAmount - sale.DiscountAmount;

            /* AUTO STOCK REDUCTION */

            part.Quantity -= sale.Quantity;

            sale.SaleDate = DateTime.Now;

            _context.Add(sale);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Purchase completed successfully!";

            return RedirectToAction(nameof(Details),
                new { id = sale.Id });
        }

        ViewBag.CustomerId =
            new SelectList(_context.Customers,
            "Id",
            "CustomerName",
            sale.CustomerId);

        ViewBag.PartId =
            new SelectList(_context.Parts,
            "Id",
            "PartName",
            sale.PartId);

        return View(sale);
    }

    // GET: SALES/Edit/5

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sale = await _context.Sales.FindAsync(id);

        if (sale == null)
        {
            return NotFound();
        }

        ViewBag.CustomerId =
            new SelectList(_context.Customers,
            "Id",
            "CustomerName",
            sale.CustomerId);

        ViewBag.PartId =
            new SelectList(_context.Parts,
            "Id",
            "PartName",
            sale.PartId);

        return View(sale);
    }

    // POST: SALES/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,CustomerId,PartId,Quantity")] Sale sale)
    {
        if (id != sale.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingSale = await _context.Sales
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == sale.Id);

                var part = await _context.Parts
                    .FirstOrDefaultAsync(p => p.Id == sale.PartId);

                if (existingSale == null || part == null)
                {
                    return NotFound();
                }

                /* RESTORE OLD STOCK */

                part.Quantity += existingSale.Quantity;

                /* CHECK NEW STOCK */

                if (sale.Quantity > part.Quantity)
                {
                    ModelState.AddModelError("",
                        "Not enough stock available.");

                    ViewBag.CustomerId =
                        new SelectList(_context.Customers,
                        "Id",
                        "CustomerName",
                        sale.CustomerId);

                    ViewBag.PartId =
                        new SelectList(_context.Parts,
                        "Id",
                        "PartName",
                        sale.PartId);

                    return View(sale);
                }

                /* REDUCE NEW STOCK */

                part.Quantity -= sale.Quantity;

                /* AUTO TOTAL */

                sale.TotalAmount =
                    part.Price * sale.Quantity;

                /* LOYALTY DISCOUNT */

                if (sale.TotalAmount > 5000)
                {
                    sale.DiscountAmount =
                        sale.TotalAmount * 0.10m;
                }
                else
                {
                    sale.DiscountAmount = 0;
                }

                /* FINAL AMOUNT */

                sale.FinalAmount =
                    sale.TotalAmount - sale.DiscountAmount;

                sale.SaleDate = existingSale.SaleDate;

                _context.Update(sale);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(sale.Id))
                {
                    return NotFound();
                }

                throw;
            }

            TempData["SuccessMessage"] =
                "Invoice updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CustomerId =
            new SelectList(_context.Customers,
            "Id",
            "CustomerName",
            sale.CustomerId);

        ViewBag.PartId =
            new SelectList(_context.Parts,
            "Id",
            "PartName",
            sale.PartId);

        return View(sale);
    }

    // GET: SALES/Delete/5

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sale = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Part)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (sale == null)
        {
            return NotFound();
        }

        return View(sale);
    }

    // POST: SALES/Delete/5

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var sale = await _context.Sales
            .Include(s => s.Part)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale != null)
        {
            /* RESTORE STOCK */

            if (sale.Part != null)
            {
                sale.Part.Quantity += sale.Quantity;
            }

            _context.Sales.Remove(sale);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Invoice deleted successfully!";

        return RedirectToAction(nameof(Index));
    }

    // EXISTS CHECK

    private bool SaleExists(int? id)
    {
        return _context.Sales.Any(e => e.Id == id);
    }
}