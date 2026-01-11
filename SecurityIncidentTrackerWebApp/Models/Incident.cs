using System.ComponentModel.DataAnnotations;

namespace SecurityIncidentTrackerWebApp.Models
{
    public class Incident
    {
        public int ID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime IncidentDate { get; set; }

        public IncidentStatus Status { get; set; }

        // Legatura cu tehnicianul care se ocupa de incident
        public int? TechnicianID { get; set; }
        public Technician? Technician { get; set; }

        // Legatura cu departamentul implicat in incident
        public int? DepartmentID { get; set; }
        public Department? Department { get; set; }

        // Legatura cu dispozitivul implicat in incident
        public int? DeviceID { get; set; }
        public Device? Device { get; set; }

        // Legatura cu tipul incidentului
        public int? IncidentTypeID { get; set; }

        [Display(Name = "Incident Type")]
        public IncidentType? IncidentType { get; set; }
    }

    public enum IncidentStatus
    {
        Open,
        [Display(Name = "In Progress")]
        InProgress,
        Resolved,
        Closed
    }
}