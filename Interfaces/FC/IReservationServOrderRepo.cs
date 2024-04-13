using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Interfaces.FC
{
	public interface IReservationServOrderRepo
	{
		IEnumerable<ReservationServiceOrderDto> Read();

		int CreateOrder(ReservationOrderDto order);

		int CreateOrderItem(ReservationServiceOrderDto orderItem);

		IEnumerable<ReservationItem> ReadByDateTime(DateTime Date, int TimePeriodId);

	}
}
