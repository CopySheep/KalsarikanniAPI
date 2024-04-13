using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dto.Jill
{
    public class HallItemDto
    {
        public int Id { get; set; }

        public string HallName { get; set; }

        public string Capacity { get; set; }

        public decimal MinRent { get; set; }

        public string Description { get; set; }

        public decimal MaxRent { get; set; }

        public string PhotoPath { get; set; }

        public bool HallStatus { get; set; }

        public string Ddescription { get; set; }
        public string Location { get; set; }

        public virtual ICollection<HallLog> HallLogs { get; set; } = new List<HallLog>();

        public IFormFile? ImageFile { get; set; }

    }
}
