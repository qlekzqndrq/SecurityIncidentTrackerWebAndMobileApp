using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly SecurityContext _context;

        public DevicesController(SecurityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices
                                 .Include(d => d.Department) 
                                 .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDevice", new { id = device.ID }, device);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}