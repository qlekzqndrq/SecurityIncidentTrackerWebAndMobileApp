using System.ComponentModel.DataAnnotations;

namespace SecurityIncidentTrackerWebApp.Models
{
    public class Device
    {
        public int ID { get; set; }

        [Display(Name = "Device Name")]
        public string DeviceName { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        public ICollection<Incident>? Incidents { get; set; }

        public int? DepartmentID { get; set; } // Cheia externa (poate fi null initial)
        public Department? Department { get; set; } // Proprietatea de navigare
    }
}
