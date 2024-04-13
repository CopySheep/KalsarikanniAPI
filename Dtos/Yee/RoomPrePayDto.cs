using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class RoomPrePayDto : IPrePayDto
    {
        public int Index { get; set; }
        public int Price { get; set; }
        public string? SKU { get; set; }
        public string? Name { get; set; }
        public string? Info { get; set; }


        // For Room
        public int RoomTypeId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
