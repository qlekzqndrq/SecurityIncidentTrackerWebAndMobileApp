using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniciansController : ControllerBase
    {
        private readonly SecurityContext _context;

        public TechniciansController(SecurityContext context)
        {
            _context = context;
        }

        // GET: api/Technicians
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Technician>>> GetTechnicians()
        {
            // Folosim "Technicians"
            return await _context.Technicians.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Technician>> PostTechnician(Technician technician)
        {
            _context.Technicians.Add(technician);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTechnician", new { id = technician.ID }, technician);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnician(int id)
        {
            var technician = await _context.Technicians.FindAsync(id);
            if (technician == null) return NotFound();

            _context.Technicians.Remove(technician);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}