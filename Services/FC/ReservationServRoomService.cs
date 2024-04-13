using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;

namespace HotelFuen31.APIs.Services.FC
{
	public class ReservationServRoomService
	{
		private IReservationServRoomRepo _repo;
		public ReservationServRoomService(IReservationServRoomRepo repo)
		{
			_repo = repo;
		}

		public IEnumerable<ReservationServiceRoomDto> ReadBytypeId(int typeId)
		{
			return _repo.ReadBytypeId(typeId);
		}

	}
}
