using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Pages.Devices
{
    public class DeleteModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public DeleteModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Department)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                Device = device;
                _context.Devices.Remove(Device);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
