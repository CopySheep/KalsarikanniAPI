using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.FC
{
	public class ReservationOrderDto
	{
		public int Id { get; set; }

		public int? ClientId { get; set; }

		public int ReservationStatusId { get; set; }

		public DateTime CreateTime { get; set; }

		public string ClientName { get; set; }

		public string PhoneNumber { get; set; }

		public virtual ICollection<ReservationItem> ReservationItems { get; set; } = new List<ReservationItem>();

		public virtual ReservationStatus ReservationStatus { get; set; }

	}
}
