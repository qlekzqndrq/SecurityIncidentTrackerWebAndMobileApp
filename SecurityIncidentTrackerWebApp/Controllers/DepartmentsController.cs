using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly SecurityContext _context;

        public DepartmentsController(SecurityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            // Luam datele brute din DB
            var departments = await _context.Departments
                .Include(d => d.TechnicianDepartments)
                    .ThenInclude(td => td.Technician)
                .ToListAsync();

            // Mapam datele intr-o structura noua, fara referinte circulare
            var cleanResult = departments.Select(d => new Department
            {
                ID = d.ID,
                DepartmentName = d.DepartmentName,
                // Extragem doar datele de baza ale tehnicienilor
                AssignedTechnicians = d.TechnicianDepartments
                    .Select(td => new Technician
                    {
                        ID = td.Technician.ID,
                        FirstName = td.Technician.FirstName,
                        LastName = td.Technician.LastName,
                        Email = td.Technician.Email,
                        Phone = td.Technician.Phone
                        // Nu mai punem technicianDepartments aici! Astfel, taiem bucla.
                    }).ToList()
            }).ToList();

            return Ok(cleanResult);
        }

        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            if (department.AssignedTechnicians != null && department.AssignedTechnicians.Any())
            {
                department.TechnicianDepartments = new List<TechnicianDepartment>();
                foreach (var tech in department.AssignedTechnicians)
                {
                    department.TechnicianDepartments.Add(new TechnicianDepartment
                    {
                        TechnicianID = tech.ID
                    });
                    _context.Entry(tech).State = EntityState.Unchanged;
                }
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartments), new { id = department.ID }, department);
        }

        // METODA PENTRU EDITARE
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.ID) return BadRequest();

            // Incarcam departamentul existent cu tot cu legaturile vechi de tehnicieni
            var existingDept = await _context.Departments
                .Include(d => d.TechnicianDepartments)
                .FirstOrDefaultAsync(d => d.ID == id);

            if (existingDept == null) return NotFound();

            // Actualizam numele departamentului
            existingDept.DepartmentName = department.DepartmentName;

            // Stergem legaturile vechi din tabelul intermediar
            if (existingDept.TechnicianDepartments != null)
            {
                _context.TechnicianDepartments.RemoveRange(existingDept.TechnicianDepartments);
            }

            // Adaugam legaturile noi venite de pe mobil
            if (department.AssignedTechnicians != null)
            {
                foreach (var tech in department.AssignedTechnicians)
                {
                    _context.TechnicianDepartments.Add(new TechnicianDepartment
                    {
                        DepartmentID = id,
                        TechnicianID = tech.ID
                    });
                    // Ii spunem ca tehnicianul exista deja in baza de date
                    _context.Entry(tech).State = EntityState.Unchanged;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.ID == id);
        }
    }
}