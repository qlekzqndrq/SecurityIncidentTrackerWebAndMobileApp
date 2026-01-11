using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public CreateModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public IActionResult OnGet()
        {
            // Trimitem lista simpla de tehnicieni prin ViewData, exact cum o cere foreach-ul din HTML
            ViewData["AllTechnicians"] = _context.Technicians.ToList();

            return Page();
        }

        // Parametrul SelectedTechnicians se potriveste cu name="SelectedTechnicians" din input
        public async Task<IActionResult> OnPostAsync(string[] SelectedTechnicians)
        {
            var newDepartment = new Department();
            newDepartment.DepartmentName = Department.DepartmentName;

            if (SelectedTechnicians != null)
            {
                newDepartment.TechnicianDepartments = new List<TechnicianDepartment>();
                foreach (var techId in SelectedTechnicians)
                {
                    // Cream legatura folosind ID-urile trimise de checkbox
                    newDepartment.TechnicianDepartments.Add(new TechnicianDepartment
                    {
                        TechnicianID = int.Parse(techId)
                    });
                }
            }

            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}