using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Repository.FC
{
	public class ReservationServOrderEFRepo : IReservationServOrderRepo
	{
		private AppDbContext _db;
		public ReservationServOrderEFRepo(AppDbContext db)
		{
			_db = db;
		}

		public int CreateOrder(ReservationOrderDto order)
		{
			Reservation reservation = new Reservation
			{
				ClientId = order.ClientId,
				ReservationStatusId = order.ReservationStatusId,
				CreateTime = order.CreateTime,
				ClientName = order.ClientName,
				PhoneNumber = order.PhoneNumber,
			};

			_db.Reservations.Add(reservation);
			_db.SaveChanges();

			return reservation.Id;

		}

		public int CreateOrderItem(ReservationServiceOrderDto orderItem)
		{
			ReservationItem item = new ReservationItem
			{
				ReservationId = orderItem.ReservationId,
				ServiceDetailId = orderItem.ServiceDetailId,
				AppointmentDate = orderItem.AppointmentDate,
				AppointmentTimePeriodId = orderItem.AppointmentTimePeriodId,
				TotalDuration = orderItem.TotalDuration,
				RoomId = orderItem.RoomId,
				RoomStatusId = orderItem.RoomStatusId,
				Subtotal = orderItem.Subtotal
			
			};

			_db.ReservationItems.Add(item);
			_db.SaveChanges();

			return item.Id;
		}

		public IEnumerable<ReservationServiceOrderDto> Read()
		{
			var model = _db.ReservationItems
				.AsNoTracking()
				.Include(x => x.ServiceDetail)
				.Include(x => x.AppointmentTimePeriod)
				.Include(x => x.Room)
				.Include(x => x.RoomStatus)
				.Select(x => new ReservationServiceOrderDto
				{
					Id = x.Id,
					ReservationId = x.ReservationId,
					ServiceDetailId = x.ServiceDetailId,
					ServiceDetailName = x.ServiceDetail.ServiceDetailName,
					AppointmentDate = x.AppointmentDate,
					AppointmentTimePeriodId = x.AppointmentTimePeriodId,
					AppointmentTimePeriodName = x.AppointmentTimePeriod.TimePeriod,
					TotalDuration = x.TotalDuration,
					RoomId = x.RoomId,
					RoomName = x.Room.RoomName,
					RoomStatusId = x.RoomStatusId,
					RoomStatusName = x.RoomStatus.RoomStatusName,
					Subtotal = x.Subtotal

				});
			return model;
		}

		public IEnumerable<ReservationItem> ReadByDateTime(DateTime Date, int TimePeriodId)
		{
			var model = _db.ReservationItems
				.AsNoTracking()
				.Where(x => x.AppointmentDate == Date && x.AppointmentTimePeriodId == TimePeriodId);

			return model;
		}

	}
}
