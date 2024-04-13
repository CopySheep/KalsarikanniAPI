using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Chen
{
    public class RoomDetailDtos
    {
        public DateTime Date { get; set; }


        public string IsHoliday { get; set; }

        public string Description { get; set; }

        public int RoomTypeId { get; set; }

        public int Price { get; set; }

    }
}
