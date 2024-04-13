using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Jill
{
    public class CreateHallLogDto
    {
        public int HallId { get; set; }

        public int? Guests { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool BookingStatus { get; set; }

        public string Name { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public List<int?> Price { get; set; }

        public List<int> Qty { get; set; }

        public List<int?> HallMenuId { get; set; }

        public string HallName { get; set; }

        public List<int> MenuLevel { get; set; }

        public List<string> MenuName { get; set; }

    }
}
