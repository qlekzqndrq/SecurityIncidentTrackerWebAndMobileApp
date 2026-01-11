using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SecurityIncidentTrackerWebApp.Models
{
    public class Technician
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [JsonIgnore] // NU trimitem in JSON 
        public ICollection<Incident>? Incidents { get; set; }

        [JsonIgnore] // NU trimitem in JSON 
        public ICollection<TechnicianDepartment> TechnicianDepartments { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
