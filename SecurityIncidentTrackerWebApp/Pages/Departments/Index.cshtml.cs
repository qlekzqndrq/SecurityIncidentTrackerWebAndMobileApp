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
    public class IndexModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public IndexModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // Folosim .Include si ThenInclude ca sa incarcam si lista de tehnicieni pentru fiecare departament
            Department = await _context.Departments
                .Include(d => d.TechnicianDepartments)
                .ThenInclude(td => td.Technician)
                .ToListAsync();
        }
    }
}