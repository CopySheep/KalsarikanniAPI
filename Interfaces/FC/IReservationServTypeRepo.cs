using HotelFuen31.APIs.Dtos.FC;

namespace HotelFuen31.APIs.Interfaces.FC
{
	public interface IReservationServTypeRepo
	{
		int Create(ReservationServiceTypeDto entity);
		void Update(ReservationServiceTypeDto entity);
		void Delete(int id);
		ReservationServiceTypeDto Get(int id);
		IQueryable<ReservationServiceTypeDto> Read();
	}
}
