using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class CouponRCSDDto: ICoupon
    {
        public CouponRCSDDto()
        {
            
        }
        public int? Id { get; set; }

        public int? TypeId { get; set; }

        public DateTime? AllowStart { get; set; }

        public DateTime? AllowEnd { get; set; }

        public bool? Cumulative { get; set; }

        public string? Comment { get; set; }

        public int? SubId { get; set; }

        public int? RoomTypeId { get; set; }

        public int? Count { get; set; }

        public int? PercentOff { get; set; }
    }
}
