using HotelFuen31.APIs.Dtos.RenYu;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services.RenYu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace HotelFuen31.APIs.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {

        public async Task SendNotification(IEnumerable<SendedNotificationDto> dto)
        {
            await Clients.All.SendNotification(dto);
        }

        public async Task CreateNotification(SendedNotificationDto dto)
        {
            await Clients.All.CreateNotification(dto);
        }
    }
}
