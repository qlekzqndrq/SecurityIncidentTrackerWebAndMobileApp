using Microsoft.AspNetCore.SignalR; 

namespace SecurityIncidentTrackerWebApp.Hubs
{
    // Mostenire din Hub
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}