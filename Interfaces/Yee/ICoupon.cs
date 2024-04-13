namespace HotelFuen31.APIs.Interfaces.Yee
{
    public interface ICoupon
    {
        public int? Id { get; set; }

        public int? TypeId { get; set; }

        public DateTime? AllowStart { get; set; }

        public DateTime? AllowEnd { get; set; }

        public bool? Cumulative { get; set; }

        public string? Comment { get; set; }
    }
}
