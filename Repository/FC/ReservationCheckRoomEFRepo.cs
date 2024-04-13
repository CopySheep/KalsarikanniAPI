using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Repository.FC
{
	public class ReservationCheckRoomEFRepo : IReservationServRoomRepo
	{
		private AppDbContext _db;
		public ReservationCheckRoomEFRepo(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<ReservationServiceRoomDto> ReadBytypeId(int typeId)
		{
			var model = _db.ReservationRooms
				.AsNoTracking()
				.Include(r => r.RoomType)
				.Where(r => r.RoomTypeId == typeId)
				.Select(r => new ReservationServiceRoomDto
				{
					Id = r.Id,
					RoomName = r.RoomName,
					RoomTypeId = r.RoomTypeId,
					RoomTypeName = r.RoomType.RoomTypeName,
					Capacity = r.Capacity,

				});
			return model;
		}
	}
}
