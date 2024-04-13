using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Chen
{
    public class RoomTypeDtos
    {
        public int RoomTypeId { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public string BedType { get; set; }

        public int RoomCount { get; set; }


        public string ImageUrl { get; set; }
        public int WeekdayPrice { get; set; }
        public int? Size { get; set; }

        public List<RoomDetailImgDtos>? ImgList { get; set; }
    }
}
