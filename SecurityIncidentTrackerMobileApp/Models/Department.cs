namespace SecurityIncidentTrackerMobileApp.Models
{
    public class Department
    {
        public int ID { get; set; }
        public string DepartmentName { get; set; }

        // Lista de tehnicieni alocați acestui departament
        public List<Technician> AssignedTechnicians { get; set; } = new List<Technician>();

        public string AssignedTechniciansNames => AssignedTechnicians != null && AssignedTechnicians.Any()
        ? string.Join(", ", AssignedTechnicians.Select(t => t.FullName))
        : "No technicians assigned";
    }
}