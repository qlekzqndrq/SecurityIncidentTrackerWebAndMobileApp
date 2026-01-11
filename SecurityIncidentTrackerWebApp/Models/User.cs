using Microsoft.AspNetCore.Identity;

namespace SecurityIncidentTrackerWebApp.Models
{
    // Mosteneste IdentityUser ca sa primeasca automat Id, Email, PasswordHash etc.
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
    }
}