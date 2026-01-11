using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Pages.Incidents
{
    public class EditModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public EditModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
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

            var incident =  await _context.Incidents.FirstOrDefaultAsync(m => m.ID == id);
            if (incident == null)
            {
                return NotFound();
            }
            Incident = incident;
           ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "DepartmentName");
           ViewData["DeviceID"] = new SelectList(_context.Devices, "ID", "DeviceName");
           ViewData["IncidentTypeID"] = new SelectList(_context.IncidentTypes, "ID", "TypeName");
           ViewData["TechnicianID"] = new SelectList(_context.Technicians, "ID", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Incident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(Incident.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.ID == id);
        }
    }
}