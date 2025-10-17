using CricketApp.API.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CricketApp.API.Hubs
{
    public class AppHub : Hub
    {
        public async Task SendNotification(object notification)
        {
            await Clients.All.SendAsync("ReceiveNotification", notification);
        }

        // CRUD Event Broadcasts for Players
        public async Task BroadcastPlayerCreated(Player player)
        {
            await Clients.All.SendAsync("PlayerCreated", player);
        }

        public async Task BroadcastPlayerUpdated(Player player)
        {
            await Clients.All.SendAsync("PlayerUpdated", player);
        }

        public async Task BroadcastPlayerDeleted(int playerId)
        {
            await Clients.All.SendAsync("PlayerDeleted", playerId);
        }
    }
}
