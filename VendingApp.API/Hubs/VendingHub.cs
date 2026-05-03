using Microsoft.AspNetCore.SignalR;
using VendingApp.API.Models;

namespace VendingApp.API.Hubs;

public class VendingHub : Hub
{
    public async Task SendVendingUpdate(VendingMachine machine)
    {
        await Clients.All.SendAsync("VendingUpdated", machine);
    }
}
