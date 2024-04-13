using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.FC
{
	public class ReservationServiceRoomDto
	{
		public int Id { get; set; }

		public string RoomName { get; set; }

		public int RoomTypeId { get; set; }
		public string RoomTypeName { get; set; }

		public int Capacity { get; set; }

		public virtual ICollection<ReservationItem> ReservationItems { get; set; } = new List<ReservationItem>();

		public virtual ReservationRoomType RoomType { get; set; }

	}
}
