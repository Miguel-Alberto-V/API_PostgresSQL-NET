using Microsoft.AspNetCore.SignalR;

namespace MyMicroservice.Hubs
{
    public class VisitasHub : Hub
    {
        public async Task NotifyVisita(string mensaje)
        {
            await Clients.All.SendAsync("ReceiveVisita", mensaje);
        }
    }
}
