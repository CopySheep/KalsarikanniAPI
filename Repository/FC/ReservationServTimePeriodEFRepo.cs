using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;


namespace HotelFuen31.APIs.Repository.FC
{
	public class ReservationServTimePeriodEFRepo : IReservationServTimePeriodRepo
	{
		private AppDbContext _db;
		public ReservationServTimePeriodEFRepo(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<ReservationServiceTimePeriodDto> ReadTime()
		{
			var model = _db.ReservationAppointmentTimePeriods
				.AsNoTracking()
				.Select(x => new ReservationServiceTimePeriodDto
				{
					Id = x.Id,
					TimePeriod = x.TimePeriod,
				});
			return model;
				
		}
	}
}
