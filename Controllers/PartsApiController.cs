using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Data;
using VehiclePartsManagementSystem.Models;

namespace VehiclePartsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PartsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PartsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Part>>> GetParts()
        {
            return await _context.Parts
                .Include(p => p.Supplier)
                .ToListAsync();
        }

        // GET: api/PartsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Part>> GetPart(int id)
        {
            var part = await _context.Parts
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (part == null)
            {
                return NotFound();
            }

            return part;
        }

        // POST: api/PartsApi
        [HttpPost]
        public async Task<ActionResult<Part>> PostPart(Part part)
        {
            _context.Parts.Add(part);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPart),
                new { id = part.Id }, part);
        }

        // PUT: api/PartsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPart(int id, Part part)
        {
            if (id != part.Id)
            {
                return BadRequest();
            }

            _context.Entry(part).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Parts.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/PartsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart(int id)
        {
            var part = await _context.Parts.FindAsync(id);

            if (part == null)
            {
                return NotFound();
            }

            _context.Parts.Remove(part);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}