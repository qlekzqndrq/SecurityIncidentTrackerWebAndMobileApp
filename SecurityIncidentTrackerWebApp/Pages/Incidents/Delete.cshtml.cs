using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Pages.Incidents
{
    public class DeleteModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public DeleteModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Incident Incident { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Incident = await _context.Incidents
                .Include(i => i.Department)   
                .Include(i => i.Device)       
                .Include(i => i.IncidentType) 
                .Include(i => i.Technician)   
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Incident == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident != null)
            {
                Incident = incident;
                _context.Incidents.Remove(Incident);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}