using System.ComponentModel.DataAnnotations;

namespace SecurityIncidentTrackerWebApp.Models
{
    public class IncidentType
    {
        public int ID { get; set; }

        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        [Display(Name = "Estimated Duration (minutes)")]
        public int EstimatedDuration { get; set; }

        public ICollection<Incident>? Incidents { get; set; }
    }
}
