using Bogus;
using Microsoft.AspNetCore.SignalR;

namespace ReportingSystem.Hubs
{
    public class StatusHub : Hub
    {
        public async Task SendStatus(string status, int percent)
        {
            await Clients.All.SendAsync("ReceiveStatus", status, percent);
        }
    }
}
