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
    public class DeleteModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public DeleteModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Folosim .Include ca sa aducem si tehnicienii alocati departamentului 
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department != null)
            {
                Department = department;

                // Cand stergem departamentul, SQL sterge automat si legaturile din tabela de mijloc
                _context.Departments.Remove(Department);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
