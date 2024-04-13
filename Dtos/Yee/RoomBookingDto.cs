using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class RoomBookingDto
    {
        public int BookingId { get; set; }

        public int OrderId { get; set; }

        public int RoomId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int? MemberId { get; set; }

        public string Remark { get; set; }

        public int BookingStatusId { get; set; }

        public DateTime? BookingCancelDate { get; set; }

        public DateTime BookingDate { get; set; }

        public int OrderPrice { get; set; }

        public string Phone { get; set; }

        public int RoomTypeId { get; set; }

        public string Name { get; set; }
    }
}
