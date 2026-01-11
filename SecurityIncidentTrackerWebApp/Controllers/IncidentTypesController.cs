using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentTypesController : ControllerBase
    {
        private readonly SecurityContext _context;

        public IncidentTypesController(SecurityContext context)
        {
            _context = context;
        }

        // GET: api/IncidentTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidentType>>> GetIncidentTypes()
        {
            return await _context.IncidentTypes.ToListAsync();
        }

        // POST: api/IncidentTypes
        [HttpPost]
        public async Task<ActionResult<IncidentType>> PostIncidentType(IncidentType incidentType)
        {
            _context.IncidentTypes.Add(incidentType);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetIncidentType", new { id = incidentType.ID }, incidentType);
        }

        // DELETE: api/IncidentTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncidentType(int id)
        {
            var incidentType = await _context.IncidentTypes.FindAsync(id);
            if (incidentType == null) return NotFound();

            _context.IncidentTypes.Remove(incidentType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
