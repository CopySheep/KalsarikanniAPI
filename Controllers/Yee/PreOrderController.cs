using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Interface.Guanyu;
using HotelFuen31.APIs.Services.Guanyu;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelFuen31.APIs.Controllers.Yee
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreOrderController : ControllerBase
    { 
        private PreOrderService _preOrderService;
        private readonly IUser _userService;
        public PreOrderController(PreOrderService preOrderService, IUser userService)
        {
            _preOrderService = preOrderService;
            _userService = userService;
        }


        // GET: api/Order/user
        [HttpGet]
        public ActionResult GetUserBudget()
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                var contextDto = _preOrderService.GetUserBudget(phone);
                var obj = new
                {
                    success = true,
                    result = contextDto,
                };

                //var jsonString = JsonConvert.SerializeObject(obj);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                return "401";
            }

            // 驗證 token 有沒有效
            string phone = _userService.GetMemberPhone(token);
            return phone;
        }
    }
}
