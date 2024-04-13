using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Interface.Guanyu;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;

namespace HotelFuen31.APIs.Controllers.Yee
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartRoomService _cartRoomService;

        private readonly IUser _userService;

        public CartController(CartRoomService service, IUser userService)
        {
            _cartRoomService = service;
            _userService = userService;
        }

        // GET: api/Cart/list
        [HttpGet]
        [Route("list")]
        public ActionResult<IEnumerable<CartRoomItemDto>> GetCartListUser()
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                var cartList = _cartRoomService.CartListUser(phone).ToList();

                cartList.ForEach(item =>
                {
                    var pic = string.IsNullOrEmpty(item.Picture) ? "noImage.png" : item.Picture;
                    item.Picture = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Content($"~/StaticFiles/Chen/{pic}")}";
                });

                return cartList;
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/cart/merge
        [HttpPost]
        [Route("merge")]
        public ActionResult PostMergeCart([FromBody] IEnumerable<CartRoomItemDto> dtos)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                _cartRoomService.MergeCart(phone, dtos);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/cart/
        [HttpPost]
        public ActionResult PostCreate([FromBody] CartRoomItemDto dto)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                int newId = _cartRoomService.CreateItem(phone, dto);
                if(newId > 0) return Ok();

                return BadRequest("加入購物車失敗");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/cart/
        [HttpDelete]
        public ActionResult DeleteItem([FromForm] string uId)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                _cartRoomService.DeleteItem(phone, uId);

                return Ok("刪除成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/cart/selected
        [HttpPut]
        [Route("selected")]
        public ActionResult PutSelectedItem([FromBody] CartRoomItemDto dto)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                _cartRoomService.SelectedItem(phone, dto);

                return Ok("選擇成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/cart/checkAll
        [HttpPut]
        [Route("checkAll")]
        public ActionResult PutCheckAll([FromForm] bool selected)
        {
            try
            {
                string phone = ValidateToken();
                if (phone == "401") return Unauthorized();

                _cartRoomService.CheckAll(phone, selected);

                return Ok("全選成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/cart/roomStock?start=2024-01-01?end=2024-01-03
        [HttpGet]
        [Route("roomStock")]
        public ActionResult<RoomStockInfo>? GetRoomStock([FromQuery] string checkInDate, string checkOutDate)
        {
            try
            {
                var info = _cartRoomService.GetRoomStock(checkInDate, checkOutDate);
                if (info == null) return null;

                info.RoomStocks?.ToList().ForEach(ele =>
                {
                    var pic = string.IsNullOrEmpty(ele.Picture) ? "noImage.png" : ele.Picture;
                    ele.Picture = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Content($"~/StaticFiles/Chen/{pic}")}";
                });

                return info;

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
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
