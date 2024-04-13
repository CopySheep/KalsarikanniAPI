using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class CartContext
    {
        public readonly List<IPrePayDto> PurchasedItems = new List<IPrePayDto>();
        public readonly List<Discount> AppliedDiscounts = new List<Discount>();
        public decimal TotalPrice = 0m;
    }
}
