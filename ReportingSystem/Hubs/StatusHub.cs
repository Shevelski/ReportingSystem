using Bogus;
using Microsoft.AspNetCore.SignalR;

namespace ReportingSystem.Hubs
{
    public class StatusHub : Hub
    {
        public async Task SendStatus(string status, int percent, string customer)
        {
            await Clients.All.SendAsync("ReceiveStatus", status, percent, customer);
        }
        public async Task SendStatus1(string customer)
        {
            await Clients.All.SendAsync("ReceiveStatus1", customer);
        }
    }
}
