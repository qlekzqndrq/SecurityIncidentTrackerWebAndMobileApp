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

namespace SecurityIncidentTrackerWebApp.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly SecurityIncidentTrackerWebApp.Data.SecurityContext _context;

        public EditModel(SecurityIncidentTrackerWebApp.Data.SecurityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public List<AssignedTechnicianData> AssignedTechnicianDataList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Department = await _context.Departments
                .Include(d => d.TechnicianDepartments)
                .ThenInclude(td => td.Technician)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Department == null) return NotFound();

            PopulateAssignedTechnicianData(Department);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedTechnicians)
        {
            if (id == null) return NotFound();

            // Incarcam obiectul din baza de date
            var departmentToUpdate = await _context.Departments
                .Include(i => i.TechnicianDepartments)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (departmentToUpdate == null) return NotFound();

            // Luam numele direct din formular, fara validari complicate
            departmentToUpdate.DepartmentName = Department.DepartmentName;

            // Actualizam checkbox-urile
            UpdateDepartmentTechnicians(selectedTechnicians, departmentToUpdate);

            // Salvam
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Daca crapa ceva, nu reincarca pagina fara sa ne afiseze un mesaj  de eroare
                ModelState.AddModelError("", "Eroare la salvare: " + ex.Message);
            }

            // Daca ajunge aici, a aparut o eroare si vom reincarca lista ca sa nu crape pagina
            PopulateAssignedTechnicianData(departmentToUpdate);
            return Page();
        }

        private void PopulateAssignedTechnicianData(Department department)
        {
            var allTechnicians = _context.Technicians;
            var departmentTechnicians = new HashSet<int>(
                department.TechnicianDepartments.Select(c => c.TechnicianID));

            AssignedTechnicianDataList = new List<AssignedTechnicianData>();

            foreach (var tech in allTechnicians)
            {
                AssignedTechnicianDataList.Add(new AssignedTechnicianData
                {
                    TechnicianID = tech.ID,
                    FullName = tech.FullName,
                    Assigned = departmentTechnicians.Contains(tech.ID)
                });
            }
        }

        private void UpdateDepartmentTechnicians(string[] selectedTechnicians, Department departmentToUpdate)
        {
            if (selectedTechnicians == null)
            {
                departmentToUpdate.TechnicianDepartments = new List<TechnicianDepartment>();
                return;
            }

            var selectedTechniciansHS = new HashSet<string>(selectedTechnicians);

            if (departmentToUpdate.TechnicianDepartments == null)
                departmentToUpdate.TechnicianDepartments = new List<TechnicianDepartment>();

            var currentTechnicians = new HashSet<int>(
                departmentToUpdate.TechnicianDepartments.Select(c => c.TechnicianID));

            foreach (var tech in _context.Technicians)
            {
                if (selectedTechniciansHS.Contains(tech.ID.ToString()))
                {
                    if (!currentTechnicians.Contains(tech.ID))
                    {
                        departmentToUpdate.TechnicianDepartments.Add(new TechnicianDepartment
                        {
                            DepartmentID = departmentToUpdate.ID,
                            TechnicianID = tech.ID
                        });
                    }
                }
                else
                {
                    if (currentTechnicians.Contains(tech.ID))
                    {
                        var remove = departmentToUpdate.TechnicianDepartments
                            .FirstOrDefault(i => i.TechnicianID == tech.ID);
                        if (remove != null) _context.Remove(remove);
                    }
                }
            }
        }
    }

    public class AssignedTechnicianData
    {
        public int TechnicianID { get; set; }
        public string FullName { get; set; }
        public bool Assigned { get; set; }
    }
}