namespace SecurityIncidentTrackerMobileApp.Models
{
    public class Device
    {
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public string SerialNumber { get; set; }

        public int? DepartmentID { get; set; }
        public Department? Department { get; set; }
    }
}