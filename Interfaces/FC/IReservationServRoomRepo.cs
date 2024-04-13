using HotelFuen31.APIs.Dtos.FC;

namespace HotelFuen31.APIs.Interfaces.FC
{
	public interface IReservationServRoomRepo
	{
		IEnumerable<ReservationServiceRoomDto> ReadBytypeId(int typeId);

	}
}
