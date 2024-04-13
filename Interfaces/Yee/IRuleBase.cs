using HotelFuen31.APIs.Dtos.Yee;

namespace HotelFuen31.APIs.Interfaces.Yee
{
    public interface IRuleBase
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Note { get; set; }
        public abstract IEnumerable<Discount> Process(CartContext cart);
    }
}
