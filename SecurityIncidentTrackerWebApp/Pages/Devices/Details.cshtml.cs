using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;
using Device = SecurityIncidentTrackerWebApp.Models.Device;

namespace SecurityIncidentTrackerWebApp.Pages.Devices
{
    public class DetailsModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public DetailsModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(m => m.Department)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (device == null)
            {
                return NotFound();
            }
            else
            {
                Device = device;
            }
            return Page();
        }
    }
}