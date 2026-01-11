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
    public class DetailsModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public DetailsModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

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
    }
}
