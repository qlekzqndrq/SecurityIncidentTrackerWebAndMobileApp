namespace SecurityIncidentTrackerWebApp.Models
{
    public class TechnicianDepartment
    {
        public int ID { get; set; }

        // Cheia catre Tehnician
        public int TechnicianID { get; set; }
        public Technician Technician { get; set; }

        // Cheia catre Departament
        public int DepartmentID { get; set; }
        public Department DepartmentName { get; set; }
    }
}
