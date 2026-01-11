namespace SecurityIncidentTrackerMobileApp.Models
{
    // Trebuie sa definim si enum-ul pe mobil ca sa inteleaga statusul
    public enum IncidentStatus
    {
        Open,
        InProgress,
        Resolved,
        Closed
    }

    public class Incident
    {
        public int ID { get; set; }

        // Proprietatea Description ("Problem Description")
        public string Description { get; set; }

        public DateTime IncidentDate { get; set; }

        // Folosim enum-ul definit mai sus
        public IncidentStatus Status { get; set; }

        public int? TechnicianID { get; set; }
        public int? DepartmentID { get; set; }

        public int? DeviceID { get; set; }
        public int? IncidentTypeID { get; set; }
    }
}