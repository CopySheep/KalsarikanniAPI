using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Interface.Guanyu;
using HotelFuen31.APIs.Interfaces.Yee;
using HotelFuen31.APIs.Services.Guanyu;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFuen31.APIs.Controllers.Yee
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private CouponService _couponService;
        private readonly IUser _userService;

        public CouponsController(CouponService couponService, IUser userService)
        {
            _couponService = couponService;
            _userService = userService;
        }

        // GET: api/Coupons/user
        [HttpGet]
        [Route("user")]
        public ActionResult<Object> GetUserCoupons()
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                var coupons = _couponService.GetUserCoupons(phone);

                return new
                {
                    Success = true,
                    result = coupons,
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Coupons/types
        [HttpGet]
        [Route("types")]
        public ActionResult<object> GetTypes()
        {
            try
            {

                var couponTypes = _couponService.GetCouponTypes();
                var roomTypes = _couponService.GetRoomTypes();
                var memberLevels = _couponService.GetMemberLevels();

                return new {
                    Success = true,
                    result = new
                    {
                        couponTypes,
                        roomTypes,
                        memberLevels,
                    },
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Coupons/user
        [HttpGet]
        [Route("all")]
        public ActionResult<Object> GetAllCoupons()
        {
            try
            {
                var coupons = _couponService.GetAllCoupons().OrderByDescending(c => c.Id);

                return new
                {
                    Success = true,
                    result = coupons,
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Post: api/Coupons/
        [HttpPost]
        public ActionResult<object> PostNewCoupon([FromForm] IFormCollection form)
        {
            try
            {
                int newId = _couponService.CreateCoupoon(form);

                return new
                {
                    Success = true,
                    result = $"優惠券新增成功: 編號 {newId}",
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("send/memberlevel")]
        public ActionResult<object> PostSendCouponByLevel([FromForm] int memberLevelId, [FromForm] int couponId)
        {
            try
            {
                 _couponService.SendCouponMemberLevel(memberLevelId, couponId);

                return new
                {
                    Success = true,
                    result = $"優惠券發送成功: MemberLevel",
                };
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
