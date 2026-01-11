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
    public class IndexModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public IndexModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        public IList<Incident> Incident { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Incident = await _context.Incidents
                .Include(i => i.Department)
                .Include(i => i.Device)
                .Include(i => i.IncidentType)
                .Include(i => i.Technician).ToListAsync();
        }
    }
}