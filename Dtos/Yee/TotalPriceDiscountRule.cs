using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class TotalPriceDiscountRule : IRuleBase
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Note { get; set; }

        public readonly decimal MinDiscountPrice = 0;

        public readonly decimal DiscountAmount = 0;

        public TotalPriceDiscountRule(int id, decimal minPrice, decimal discount)
        {
            this.Id = id;
            this.Name = $"折價券滿 {minPrice} 抵用 {discount}";
            this.Note = $"消費滿額折抵";
            this.MinDiscountPrice = minPrice;
            this.DiscountAmount = discount;
        }

        public IEnumerable<Discount> Process(CartContext cart)
        {
            if (cart.TotalPrice >= this.MinDiscountPrice) yield return new Discount()
            {
                Amount = this.DiscountAmount,
                Rule = this,
                Products = cart.PurchasedItems.ToArray(),
            };
        }
    }
}
