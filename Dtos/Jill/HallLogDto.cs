using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Jill
{
    public class HallLogDto
    {
        public int Id { get; set; }
        public int HallId { get; set; }

        public int? UserId { get; set; }

        public int? Guests { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool BookingStatus { get; set; }

        public string Name { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string FilePath { get; set; }

        public string HallName { get; set; }

        public virtual HallItem Hall { get; set; }
        public virtual ICollection<HallOrderItem> HallOrderItems { get; set; } = new List<HallOrderItem>();
    }
}
