namespace SecurityIncidentTrackerMobileApp.Models
{
    public class Technician
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Proprietate calculata 
        public string FullName => $"{FirstName} {LastName}";

        public bool IsSelected { get; set; }
    }
}