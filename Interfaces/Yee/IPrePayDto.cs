
namespace HotelFuen31.APIs.Interfaces.Yee
{
    public interface IPrePayDto
    {
        public int Index { get; set; }

        public int Price { get; set; }

        public string? SKU { get; set; }

        public string? Name { get; set; }

        public string? Info { get; set; }
    }
}
