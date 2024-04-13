using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class POS
    {
        public readonly List<IRuleBase> ActivedRules = new List<IRuleBase>();

        public bool CheckoutProcess(CartContext cart)
        {
            // reset cart
            cart.AppliedDiscounts.Clear();

            cart.TotalPrice = cart.PurchasedItems.Select(p => p.Price).Sum();
            foreach (var rule in this.ActivedRules)
            {
                var discounts = rule.Process(cart);
                cart.AppliedDiscounts.AddRange(discounts);
                cart.TotalPrice -= discounts.Select(d => d.Amount).Sum();
            }
            return true;
        }
    }
}
