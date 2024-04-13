using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class PreOrderDto
    {
        public List<IPrePayDto>? PurchasedItems { get; set; }
        public List<DiscountDto>? AppliedDiscounts { get; set; }
        public decimal? OriginalPrice => this.PurchasedItems?.Sum(pi => pi.Price);
        public decimal? Discount => this.AppliedDiscounts?.Sum(ad => ad.Amount);
        public decimal? FinalPrice => this.OriginalPrice - this.Discount;
    }
}
