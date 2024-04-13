using HotelFuen31.APIs.Dtos.Haku;
using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Interface.Guanyu;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services.Guanyu;
using HotelFuen31.APIs.Services.Haku;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFuen31.APIs.Controllers.Haku
{
    [Route("api/[controller]")]
	[ApiController]
	public class DispatchController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IUser _userService;
		private readonly DispatchService _dispatchService;

		public DispatchController(AppDbContext context, IUser userService, DispatchService service)
		{
			_context = context;
			_userService = userService;
			_dispatchService = service;
		}

		// 取得最新的搭車訂單細項列表
		// GET: api/Dispatch/list
		[HttpGet]
		[Route("list")]
		public ActionResult<IEnumerable<CarTaxiOrderItemDto>> GetOrderListUser()
		{
			try
			{
				string phone = ValidateToken();
				if (phone == "401") return Unauthorized();

				var orderList = _dispatchService.OrderListUser(phone).ToList();

				// 測試用
				//string phone = "test";
				//int memberId = 1;
				// 測試用

				//var orderList = _dispatchService.OrderListUser(phone).ToList();

				return orderList;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST: api/Dispatch/avaList
		[HttpPost]
		[Route("avaList")]
		public ActionResult<IEnumerable<CarsDto>> searchAvailables([FromBody] CarTaxiOrderItemDto dto)
		{
			try
			{
				var carsDtos = _dispatchService.RemainingCars(dto).ToList();
				return carsDtos;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST: api/Dispatch/
		[HttpPost]
		public ActionResult PostCreate([FromBody] CarTaxiOrderItemDto dto)
		{
			try
			{
				//string phone = dto.PhoneNumber;
				string phone = ValidateToken();
				if (phone == "401") return Unauthorized();

				//string phone = "test";//測試用

				int newId = _dispatchService.CreateItem(phone, dto);
				if (newId > 0) return Content("成功");

				return Content("預訂搭乘失敗");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/Dispatch/5
		[HttpDelete("{id}")]
		public ActionResult DeleteCarOrder(int id)
		{
			string result= _dispatchService.CarOrderDelete(id);
			return Content(result);
		}

		private string ValidateToken()
		{
			// 取得 request 置於 Header 中的 token
			string? authorization = HttpContext.Request.Headers["Authorization"];
			if (string.IsNullOrWhiteSpace(authorization))
			{
				return "401";
			}

			// 將字串中的 token 拆出來
			string token = authorization.Split(" ")[1];
			if (string.IsNullOrWhiteSpace(token))
			{
				throw new ArgumentException("Invalid Authorization token format.");
			}

			// 驗證 token 有沒有效
			string phone = _userService.GetMemberPhone(token);
			return phone;
		}
	}
}
