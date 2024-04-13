using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.FC
{
	public class ReservationServiceOrderDto
	{
		public int Id { get; set; }

		public int ReservationId { get; set; }

		public int ServiceDetailId { get; set; }
		public string ServiceDetailName { get; set; }


		public DateTime AppointmentDate { get; set; }

		public int AppointmentTimePeriodId { get; set; }
		public string AppointmentTimePeriodName { get; set; }


		public int TotalDuration { get; set; }

		public int? RoomId { get; set; }

		public string RoomName { get; set; }

		public int? RoomStatusId { get; set; }
		public string RoomStatusName { get; set; }



		public int Subtotal { get; set; }

		public virtual ReservationAppointmentTimePeriod AppointmentTimePeriod { get; set; }

		public virtual Reservation Reservation { get; set; }

		public virtual ReservationRoom Room { get; set; }

		public virtual ReservationRoomStatus RoomStatus { get; set; }

		public virtual ReservationServiceDetail ServiceDetail { get; set; }

	}
}
