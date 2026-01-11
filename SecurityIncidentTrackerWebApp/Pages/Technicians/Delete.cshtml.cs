using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Pages.Technicians
{
    public class DeleteModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public DeleteModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Technician Technician { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technicians.FirstOrDefaultAsync(m => m.ID == id);

            if (technician == null)
            {
                return NotFound();
            }
            else
            {
                Technician = technician;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technicians.FindAsync(id);
            if (technician != null)
            {
                Technician = technician;
                _context.Technicians.Remove(Technician);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
