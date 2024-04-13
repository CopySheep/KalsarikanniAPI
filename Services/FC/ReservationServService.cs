using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;
using HotelFuen31.APIs.Repository.FC;
using System.Security.Policy;

namespace HotelFuen31.APIs.Services.FC
{
	public class ReservationServService
	{
		private IReservationServRepo _repo;
		public ReservationServService(IReservationServRepo repo)
		{
			_repo = repo;
		}
		public IEnumerable<ReservationServiceDetailDto> Read()
		{
            return _repo.Read();
		}
		public IQueryable<ReservationServiceDetailDto> GetByid(int id)
		{
			return _repo.GetByid(id);
		}
	}
}
