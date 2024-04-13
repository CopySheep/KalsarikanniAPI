namespace HotelFuen31.APIs.Dtos.Chen
{
    public class CheckRoomDto
    {
        public int RoomTypeId { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public string BedType { get; set; }

        public int SumPrice { get; set; }


        public string ImageUrl { get; set; }
        public int? Size { get; set; }
        public int CanSoldQty { get; set; }
    }
}
