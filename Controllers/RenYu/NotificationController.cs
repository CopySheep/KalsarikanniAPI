using Hangfire;
using HotelFuen31.APIs.Dtos.RenYu;
using HotelFuen31.APIs.Hubs;
using HotelFuen31.APIs.Interface.Guanyu;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services.RenYu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HotelFuen31.APIs.Controllers.RenYu
{
    // https://eugenesu.me/2021/08/27/hangfire-entry/
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _service;
        private IHubContext<NotificationHub, INotificationHub> _hub;
        private readonly IUser _user;

        public NotificationController(NotificationService service, IHubContext<NotificationHub, INotificationHub> hub, IUser user)
        {
            _service = service;
            _hub = hub;
            _user = user;
        }

        [HttpGet]
        public AllNotificationPageDto GetAll(int page = 1)
        {
            return _service.GetAll(page);
        }

        [HttpGet("GetLevels")]
        public async Task<IEnumerable<MemberLevel>> GetLevels()
        {
            return await _service.GetLevels().ToListAsync();
        }
        [HttpGet("GetTypes")]
        public async Task<IEnumerable<NotificationType>> GetTypes()
        {
            return await _service.GetTypes().ToListAsync();
        }

        // POST: api/Notification/list
        [HttpPost("list")]
        public ActionResult<NotificationPagesDto> GetAllNotification(int page = 1)
        {
            try
            {
                string idStr = ValidateToken();
                if(idStr == "401")
                {
                    return Unauthorized();
                }

                int id = int.Parse(idStr);

                return _service.GetAllNotifications(id,page);
            }
            catch (Exception ex) 
            { 
               return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<IEnumerable<SendedNotificationDto>> SendLatestNotifiction()
        {   
            try
            {
                string idStr = ValidateToken();

                int id = int.Parse(idStr);
               
                var dto = _service.GetLatestNotifications(id).ToList();

                _hub.Clients.All.SendNotification(dto);
                
                return Ok(dto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<string> CreateNotifiction(SendedNotificationDto dto)
        {
            return await _service.Create(dto); 
        }

        private string ValidateToken()
        {
            string? authoriztion = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authoriztion))
            {
                return "401";
            }

            string token = authoriztion.Split(" ")[1];

            if (string.IsNullOrEmpty(token))
            {
                return "401";
            }

            string id = _user.GetMember(token);

            return id;
        }

        [HttpGet("GetOneNotfication")]
        public IEnumerable<NotificationDto> GetNotification(int id) 
        {
            return _service.GetNotification(id);
        }

        [HttpPut("Edit")]
        public async Task<string> Edit(NotificationDto dto)
        {
            return await _service.Edit(dto);
        }
    }
}
