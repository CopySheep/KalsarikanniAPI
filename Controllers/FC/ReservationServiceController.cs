using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Services.FC;
using HotelFuen31.APIs.Services.RenYu;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HotelFuen31.APIs.Controllers.FC
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationServiceController : ControllerBase
	{
		private readonly ReservationServTimePeriodService _roomService;
		private readonly ReservationServOrderService _orderService;

		private readonly SendEmailService _sendEmailService;

		public ReservationServiceController(ReservationServTimePeriodService roomService, ReservationServOrderService orderService, SendEmailService sendEmailService)
		{
			_roomService = roomService;
			_orderService = orderService;
			_sendEmailService = sendEmailService;
		}

		[HttpGet]
		public  IEnumerable<ReservationServiceTimePeriodDto> CheckRoomCount(int typeId, DateTime? selectdate)
		{
			if (selectdate == null) selectdate = DateTime.Today;
			var dtos = _roomService.ReadTime(typeId, selectdate);

			var model = dtos.Select(x => new ReservationServiceTimePeriodDto
			{
				Id = x.Id,
				TimePeriod = x.TimePeriod,
				Count = x.Count-1
			});

			return model;
		}

		[HttpPost]
		public ActionResult CreateOrderAll([FromBody] ReservationVueDto dto)
		{
			try
			{
				int newId = _orderService.Create(dto);
				if (newId > 0)
				{
					var title = $@"親愛的 {dto.ClientName} 您好，<br/>您的預約明細如下:";
					var htmlContent = GenerateHtmlTable(dto);

					_sendEmailService.SendEmail("預約訂單通知信", title, htmlContent, dto.Email);

					return Ok(new { id = newId });
				}
				else
				{
					return BadRequest("Failed to create order.");
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		public static string GenerateHtmlTable(ReservationVueDto dto)
		{
			var content = new StringBuilder();
			content.Append("<table style=\"border: 1px solid black; border-collapse: collapse;  width:100%;\">"); // 添加表格边框样式
			content.Append("<tr style=\"background-color: #9b7c6487;\">" +
				"<th style=\"border: 1px solid black; text-align: center;\">手機號碼</th>" +
				"<th style=\"border: 1px solid black; text-align: center;\">服務項目</th>" +
				"<th style=\"border: 1px solid black; text-align: center;\">預約日期</th>" +
				"<th style=\"border: 1px solid black; text-align: center;\">預約時段</th>" +
				"<th style=\"border: 1px solid black; text-align: center;\">療程時數</th>" +
				"<th style=\"border: 1px solid black; text-align: center;\">小計</th>" +
				"</tr>");

			// 格式化AppointmentDate为YYYY-MM-DD
			string formattedAppointmentDate = dto.AppointmentDate.ToString("yyyy-MM-dd");

			content.Append($"<tr>" +
				$"<td style=\"border: 1px solid black; text-align: center;\">{dto.PhoneNumber}</td>" +
				$"<td style=\"border: 1px solid black; text-align: center;\">{dto.ServiceDetailName}</td>" +
				$"<td style=\"border: 1px solid black; text-align: center;\">{formattedAppointmentDate}</td>" +
				$"<td style=\"border: 1px solid black; text-align: center;\">{dto.AppointmentTimePeriodName}</td>" +
				$"<td style=\"border: 1px solid black; text-align: center;\">{dto.TotalDuration}</td>" +
				$"<td style=\"border: 1px solid black; text-align: center;\">{dto.Subtotal}</td>" +
				$"</tr>");

			content.Append("</table>" +
				"<br><br>如果您有任何問題或特殊需求，請隨時與我們聯繫。" +
				"<br>歡迎來信：KALSARIHOTEL@gmail.com" +
				"<br>歡迎致電：+886 2-2592-6998");
			return content.ToString();
		}
	}
}
