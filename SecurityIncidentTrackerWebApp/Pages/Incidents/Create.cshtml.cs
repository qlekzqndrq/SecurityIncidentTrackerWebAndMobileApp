using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR; 
using SecurityIncidentTrackerWebApp.Data;
using SecurityIncidentTrackerWebApp.Models;
using SecurityIncidentTrackerWebApp.Hubs; 

namespace SecurityIncidentTrackerWebApp.Pages.Incidents
{
    public class CreateModel : PageModel
    {
        private readonly SecurityContext _context;
        private readonly IHubContext<NotificationHub> _hubContext; // Hub pentru notificari

        // Constructorul primeste si hub-ul
        public CreateModel(SecurityContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "DepartmentName");
            ViewData["DeviceID"] = new SelectList(_context.Devices, "ID", "DeviceName");
            ViewData["IncidentTypeID"] = new SelectList(_context.IncidentTypes, "ID", "TypeName");
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "ID", "FullName");
            return Page();
        }

        [BindProperty]
        public Incident Incident { get; set; } = default!;

        // POST
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Salvam incidentul
            _context.Incidents.Add(Incident);
            await _context.SaveChangesAsync();

            // Trimitem notificarea catre aplicatiile conectate
            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Incident nou: {Incident.Description}");
            }

            // Redirectionam inapoi la lista
            return RedirectToPage("./Index");
        }
    }
}
