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
    public class IndexModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public IndexModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        public IList<Device> Device { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Device = await _context.Devices
                .Include(d => d.Department)
                .ToListAsync();
        }
    }
}
