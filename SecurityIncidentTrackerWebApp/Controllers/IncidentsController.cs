using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR; 
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Hubs; 
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly SecurityContext _context;
        // Variabila pentru SignalR
        private readonly IHubContext<NotificationHub> _hubContext;

        // Constructorul care primeste contextul bazei de date si Hub-ul
        public IncidentsController(SecurityContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/Incidents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        // GET: api/Incidents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            return incident;
        }

        // POST: api/Incidents
        [HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            // Trimitem notificarea AICI, dupa salvare
            // Folosim Description din model
            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Incident nou: {incident.Description}");
            }

            return CreatedAtAction("GetIncident", new { id = incident.ID }, incident);
        }

        // PUT: api/Incidents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, Incident incident)
        {
            if (id != incident.ID)
            {
                return BadRequest();
            }

            _context.Entry(incident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(id))
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

        // DELETE: api/Incidents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.ID == id);
        }
    }
}