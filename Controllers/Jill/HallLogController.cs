using HotelFuen31.APIs.Dtos.Jill;
using HotelFuen31.APIs.Dtos.RenYu;
using HotelFuen31.APIs.Services.Jill;
using HotelFuen31.APIs.Services.RenYu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace HotelFuen31.APIs.Controllers.Jill
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallLogController : ControllerBase
    {
        private readonly HallLogService _service;
        private readonly SendEmailService _mailservice;

        public HallLogController(HallLogService service, SendEmailService mailservice)
        {
            _service = service;
            _mailservice = mailservice;
        }

        // GET: api/HallLog
        [HttpGet]
        public async Task<IEnumerable<HallLogDto>> GetAll()
        {
            return await _service.GetAll().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<HallLogDto>> SearchLog(int id)
        {
            return await _service.SearchLog(id).ToListAsync();
        }

        //[HttpPost("Create")]
        //public async Task<string> CreateOrder([FromBody]CreateHallLogDto dto)
        //{
        //    return await _service.Create(dto);
        //}

        [HttpPost("Create")]
        public async Task<string> CreateOrder([FromBody] CreateHallLogDto dto)
        {
            var result = await _service.Create(dto);

            string menuContent = "";
            for (int i = 0; i < dto.MenuLevel.Count; i++)
            {
                menuContent += @$"
                <tr>
                    <td>{dto.MenuLevel[i]}</td>
                    <td>{dto.MenuName[i]}</td>
                    <td>{dto.Qty[i]}</td>
                    <td>{dto.Price[i]}</td>
                </tr>";
            }

            string title = @$"親愛的客戶 {dto.Name} 您好，<br>
                
                <br>感謝您選擇了 Kalsari Hotel 作為您舉辦宴會的地點。
                <br>如果您有任何問題或特殊需求，請隨時與我們聯繫。
                <br>
                <br>您的訂單明細如下：";

            string content = 

                @$"
                <br>預約場地：<div style=\""font-weight: bold;\"">{dto.HallName}</div>
                <br>預約開始時間：<div style=\""font-weight: bold;\"">{dto.StartTime}</div>
                <br>預約結束結束：<div style=\""font-weight: bold;\"">{dto.EndTime}</div>
                <br>預約人數：<div style=\""font-weight: bold;\"">{dto.Guests}</div>
                <br>預約人電話：<div style=\""font-weight: bold;\"">{dto.CellPhone}</div>

                <br>預約餐點內容：
                <table border=\""1\"">
                    <thead>
                        <tr>
                        <th style=\""text-align: left;\"">出餐順序</th>
                        <th style=\""text-align: left;\"">餐點名稱</th>
                        <th style=\""text-align: left;\"">數量</th>
                        <th style=\""text-align: left;\"">價錢</th>
                        </tr>
                    </thead>
                    <tbody>
                        {menuContent}
                    </tbody>
                </table>
                <br>
                <br>
                <br>
                <br>
                Kalsari Hotel 團隊 敬上
                ";


            string email = dto.Email;

            _mailservice.SendEmail("Kalsari Hotel宴會廳預定成功", title, content, email);

            return result;

        }
    }
}
