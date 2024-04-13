using HotelFuen31.APIs.Interface.Guanyu;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Controllers.Yee
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly IUser _userService;

        public OrderController(OrderService orderService, IUser userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        // POST: api/Order/create
        [HttpPost]
        [Route("create")]
        public ActionResult<Object> PostCreateOrderLogged([FromBody] int[] coupons)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                int newOrderId = _orderService.CreateOrder(phone, coupons);

                return new
                {
                   Success = true,
                   OrderId = newOrderId,
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Order/user
        [HttpGet]
        [Route("user")]
        public ActionResult<Object> GetUserOrders()
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                var order = _orderService.GetUserOrders(phone);

                return new
                {
                    Success = true,
                    result = order,
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Order/
        [HttpGet]
        public ActionResult<Object> GetOrder([FromQuery] int orderId)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                var order = _orderService.GetOrder(phone, orderId);

                return new
                {
                    Success = true,
                    result = order,
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Order/ECPay
        [HttpGet]
        [Route("ECPay")]
        public ActionResult<Object> GetECPayForm([FromQuery] int orderId)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                var orderDto = _orderService.GetOrder(phone, orderId);

                if (orderDto.RtnCode == 1 || orderDto.Status == 1) return BadRequest("該訂單已付款");

                //string backEnd = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string backEnd = $"https://5223-111-184-154-28.ngrok-free.app";
                string frontEnd = $"localhost:5173";

                var orderDic = _orderService.GetECPayDic(orderDto, backEnd, frontEnd);

                return orderDic;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Order/ECPay
        [HttpPost]
        [Route("ECPay")]
        public ActionResult PostFromECPay([FromForm]IFormCollection col)
        {
            try
            {
                var data = new Dictionary<string, string>();
                foreach (string key in col.Keys)
                {
                    data.Add(key, col[key].ToString() ?? "");
                }

                int rtnCod = _orderService.UpdateECpay(data);
                if (rtnCod == 1) return Content("1|OK");

                return BadRequest("交易失敗");
            }
            catch(Exception ex) { 
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
