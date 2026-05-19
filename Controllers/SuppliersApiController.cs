using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Data;
using VehiclePartsManagementSystem.Models;

namespace VehiclePartsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SuppliersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SuppliersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            return await _context.Suppliers
                .Include(s => s.Parts)
                .ToListAsync();
        }

        // GET: api/SuppliersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Parts)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // POST: api/SuppliersApi
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplier),
                new { id = supplier.Id }, supplier);
        }

        // PUT: api/SuppliersApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Suppliers.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/SuppliersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}