using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Pages.Departments
{
    public class DetailsModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public DetailsModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incarcam departamentul + legaturile + tehnicienii
            var department = await _context.Departments
                .Include(d => d.TechnicianDepartments)
                .ThenInclude(td => td.Technician)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (department == null)
            {
                return NotFound();
            }
            else
            {
                Department = department;
            }
            return Page();
        }
    }
}