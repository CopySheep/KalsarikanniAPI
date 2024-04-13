using HotelFuen31.APIs.Dtos.FC;

namespace HotelFuen31.APIs.Interfaces.FC
{
	public interface IReservationServRepo
	{
		int Create(ReservationServiceDetailDto entity);
		void Update(ReservationServiceDetailDto entity);
		void Delete(int id);
		ReservationServiceDetailDto Get(int id);
		IQueryable<ReservationServiceDetailDto> Read();

		IQueryable<ReservationServiceDetailDto> GetByid(int id);


		//ReservationServiceDetailDto Get(Func<Product, bool> func);

	}
}
