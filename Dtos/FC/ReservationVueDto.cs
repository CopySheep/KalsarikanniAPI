using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.FC
{
	public class ReservationVueDto
	{

		public int? ClientId { get; set; }

		public int? ReservationStatusId { get; set; }

		public DateTime? CreateTime { get; set; }

		public string ClientName { get; set; }

		public string PhoneNumber { get; set; }

		public int? ReservationId { get; set; }

		public int ServiceDetailId { get; set; }
		public string ServiceDetailName { get; set; }


		public DateTime AppointmentDate { get; set; }

		public int AppointmentTimePeriodId { get; set; }
		public string AppointmentTimePeriodName { get; set; }


		public int TotalDuration { get; set; }

		public int? RoomId { get; set; }

		//public string RoomName { get; set; }

		public int? RoomStatusId { get; set; }
		//public string RoomStatusName { get; set; }

		public int Subtotal { get; set; }

		public string Email { get; set; }

	}
}
