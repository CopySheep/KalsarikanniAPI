using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.FC
{
	public class ReservationServiceTypeDto
	{
		public int Id { get; set; }

		public string ServicesTypeName { get; set; }

		public string Description { get; set; }

		public string ImgUrl { get; set; }

		public virtual ICollection<ReservationServiceDetail> ReservationServiceDetails { get; set; } = new List<ReservationServiceDetail>();


	}
}
