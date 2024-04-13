using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class MemberCheckOutDto
    {
        public int? Id { get; set; }
        public string? Phone { get; set; }
        public IEnumerable<CartRoomItem>? Selected { get; set; }
        public IEnumerable<CouponMember>? CouponMembers { get; set; }
    }
}
