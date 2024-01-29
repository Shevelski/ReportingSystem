using Microsoft.AspNetCore.SignalR;
using ReportingSystem.Hubs;

namespace ReportingSystem.Data.Generate
{
    public class StatusSignalR(IHubContext<StatusHub> hubContext)
    {
        private readonly IHubContext<StatusHub> _hubContext = hubContext;
        public async Task UpdateStatusGenerateData(string customer)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveStatus1", customer);
        }
    }
}
