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

namespace SecurityIncidentTrackerWebApp.Pages.Devices
{
    public class EditModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public EditModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var device = await _context.Devices.FirstOrDefaultAsync(m => m.ID == id);
            if (device == null) return NotFound();

            Device = device;

            // Dropdown pentru Departamente
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "DepartmentName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "DepartmentName");
                return Page();
            }

            _context.Attach(Device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Devices.Any(e => e.ID == Device.ID)) return NotFound();
                else throw;
            }

            return RedirectToPage("./Index");
        }
    }
}