using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Pages.Devices
{
    public class CreateModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public CreateModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Incarcam lista de departamente în "ViewData"
            // "ID" este valoarea care se salveaza, "DepartmentName" este ce vede utilizatorul
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "DepartmentName");
            return Page();
        }

        [BindProperty]
        public Device Device { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Devices.Add(Device);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
