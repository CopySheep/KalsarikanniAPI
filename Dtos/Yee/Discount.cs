using HotelFuen31.APIs.Interfaces.Yee;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class Discount
    {
        public int? Id;
        public IRuleBase? Rule;
        public IPrePayDto[]? Products;
        public decimal Amount;
    }
}
