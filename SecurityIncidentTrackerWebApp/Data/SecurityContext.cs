using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Data
{
    // Mostenire din IdentityDbContext<User>
    public class SecurityContext : IdentityDbContext<User>
    {
        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options)
        {
        }

        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<Incident> Incidents { get; set; }

        public DbSet<SecurityIncidentTrackerWebApp.Models.TechnicianDepartment> TechnicianDepartments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Forteaza Identity sa foloseasca numele de tabela ales de mine
            builder.Entity<User>().ToTable("User");
        }
    }
}