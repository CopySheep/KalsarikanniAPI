using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Controllers.Yee
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private readonly CartRoomService _service;

        public DetailsController(CartRoomService service)
        {
            _service = service;
        }

        // GET: api/details/roomStock?start=2024-01-01?end=2024-01-03
        [HttpGet]
        [Route("roomStock")]
        public ActionResult<RoomStockInfo>? GetRoomStock([FromQuery] string start, string end)
        {
            try
            {
                var info = _service.GetRoomStock(start, end);
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
    }
}
