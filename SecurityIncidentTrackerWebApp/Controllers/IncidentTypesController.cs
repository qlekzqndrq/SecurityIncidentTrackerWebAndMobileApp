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

        // PUT: api/IncidentTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncidentType(int id, IncidentType incidentType)
        {
            if (id != incidentType.ID)
            {
                return BadRequest();
            }

            // Aici ii spune bazei de date ca obiectul a fost modificat
            _context.Entry(incidentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Verifica daca exista un ID
        private bool IncidentTypeExists(int id)
        {
            return _context.IncidentTypes.Any(e => e.ID == id);
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
