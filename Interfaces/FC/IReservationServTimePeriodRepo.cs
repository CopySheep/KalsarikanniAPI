using HotelFuen31.APIs.Dtos.FC;

namespace HotelFuen31.APIs.Interfaces.FC
{
	public interface IReservationServTimePeriodRepo
	{
		IEnumerable<ReservationServiceTimePeriodDto> ReadTime();
	}
}
