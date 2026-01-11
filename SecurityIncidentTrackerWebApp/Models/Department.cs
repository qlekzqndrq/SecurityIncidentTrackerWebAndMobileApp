using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityIncidentTrackerWebApp.Models
{
    public class Department
    {
        public int ID { get; set; }

        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [JsonIgnore] // NU trimitem in JSON
        public ICollection<Incident>? Incidents { get; set; }

        [JsonIgnore] // NU trimitem in JSON
        public ICollection<TechnicianDepartment> TechnicianDepartments { get; set; }

        [NotMapped]
        [JsonProperty("AssignedTechnicians")] 
        public List<Technician> AssignedTechnicians { get; set; } = new List<Technician>();
    }
}
