using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace HotelFuen31.APIs.Hubs
{
    public class LiveCustomerServiceHub: Hub<ILiveCustomerServiceHub>
    {
        public static Dictionary<string, string> userInfoDict = new Dictionary<string, string>();

        public async Task LoadUserInfo(dynamic message)
        {
            dynamic dybParam = JsonConvert.DeserializeObject(Convert.ToString(message));
            string userId = dybParam.userId;
            var Id = Context.ConnectionId;
            userInfoDict[userId] = Id;
            await Clients.Clients(Id).StringDataTransfer("Login Successfully");
        }

        public async Task SendToConnection(string userId, string message)
        {
            if(userInfoDict.ContainsKey(userId))
            {
                await Clients.Client(userInfoDict[userId]).StringDataTransfer(message);
            }
        }

        public override Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string Id = Context.ConnectionId;
            string userId = string.Empty;
            if(userInfoDict.ContainsKey(Id))
            {
                string key = userInfoDict.FirstOrDefault(x => x.Value == Id).Key;
                userInfoDict.Remove(key);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
