using HotelFuen31.APIs.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace HotelFuen31.APIs.Controllers.RenYu
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveCustomerServiceController : ControllerBase
    {
        private IHubContext<LiveCustomerServiceHub, ILiveCustomerServiceHub > _hub;

        public LiveCustomerServiceController(IHubContext<LiveCustomerServiceHub, ILiveCustomerServiceHub> hub)
        {
            _hub = hub;
        }

        [HttpPost]
        [Route("ToUser")]
        public string ToUser([FromBody] JsonElement jobJ)
        {
            var userId = jobJ.GetProperty("userId").GetString();
            var msg = jobJ.GetProperty("msg").GetString();
            if (LiveCustomerServiceHub.userInfoDict.ContainsKey(userId))
            {
                _hub.Clients.Client(LiveCustomerServiceHub.userInfoDict[userId]).StringDataTransfer(msg);
                return "Msg sent successfully to user";
            }
            else
            {
                return "Msg sent failed to user";
            }
        }
    }
}
