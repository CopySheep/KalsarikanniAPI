using System;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class CartRoomItemDto
    {
        public int? Id { get; set; }
        public string? Uid { get; set; }
        public bool Selected { get; set; }
        public int TypeId { get; set; }
        public int RoomId { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? CheckInDate { get; set; }
        public string? CheckOutDate { get; set; }
        public string? Info => $"入住時間: {this.CheckInDate}, 退房時間: {this.CheckOutDate}, 備註: {this.Remark}";
        public int Price { get; set; }
        public int Count { get; set; }
        public string? Phone { get; set; }
        public string? Remark { get; set; }
    }
}
