using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.FC
{
	public class ReservationServiceDetailDto
	{
		public int Id { get; set; }

		public int ServicesTypeId { get; set; }

		public string ServicesTypeName { get; set; }

		public string ServiceDetailName { get; set; }

		public int Time { get; set; }

		public int Price { get; set; }

		public string Description { get; set; }

		public string ImgUrl { get; set; }

		public virtual ICollection<ReservationItem> ReservationItems { get; set; } = new List<ReservationItem>();

		public virtual ReservationServicesType ServicesType { get; set; }

	}
}
