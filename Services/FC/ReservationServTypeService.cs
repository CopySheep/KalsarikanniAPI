using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;

namespace HotelFuen31.APIs.Services.FC
{
	public class ReservationServTypeService
	{
		private IReservationServTypeRepo _repo;
		public ReservationServTypeService(IReservationServTypeRepo repo)
		{
			_repo = repo;
		}
		public IQueryable<ReservationServiceTypeDto> Read()
		{
			return _repo.Read();
		}
	}
}
