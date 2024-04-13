using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class CouponRTSDto : ICoupon
    {
        public CouponRTSDto()
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

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? PercentOff  { get; set; }
    }
}
