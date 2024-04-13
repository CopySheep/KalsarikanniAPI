using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string? Phone { get; set; }

        public int Status { get; set; }

        public DateTime OrderTime { get; set; }

        public string? MerchantTradeNo { get; set; }

        public int? RtnCode { get; set; }

        public string? RtnMsg { get; set; }

        public string? TradeNo { get; set; }

        public int? TradeAmt { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? PaymentType { get; set; }

        public string? PaymentTypeChargeFee { get; set; }

        public string? TradeDate { get; set; }

        public int? SimulatePaid { get; set; }

        public virtual IEnumerable<RoomBookingDto>? RoomBookings { get; set; }
    }
}
