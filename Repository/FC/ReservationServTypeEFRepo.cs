using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Repository.FC
{
	public class ReservationServTypeEFRepo : IReservationServTypeRepo
	{
		private AppDbContext _db;
		public ReservationServTypeEFRepo(AppDbContext db)
		{
			_db = db;
		}


		public int Create(ReservationServiceTypeDto entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ReservationServiceTypeDto Get(int id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<ReservationServiceTypeDto> Read()
		{
			var model = _db.ReservationServicesTypes
				.AsNoTracking()
				.Select(r => new ReservationServiceTypeDto
				{
					Id = r.Id,
					ServicesTypeName = r.ServicesTypeName,
					Description = r.Description,
					ImgUrl = r.ServicesTypeImageUrl
				});
			return model;
		}

		public void Update(ReservationServiceTypeDto entity)
		{
			throw new NotImplementedException();
		}
	}
}
