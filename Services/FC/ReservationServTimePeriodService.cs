using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;

namespace HotelFuen31.APIs.Services.FC
{
	public class ReservationServTimePeriodService
	{
		private ReservationServRoomService _roomService;
		private ReservationServOrderService _orderService;
		private IReservationServTimePeriodRepo _TimeRepo;
		public ReservationServTimePeriodService(ReservationServRoomService roomService, IReservationServTimePeriodRepo TimeRepo, ReservationServOrderService orderService)
		{
			_roomService = roomService;
			_TimeRepo = TimeRepo;
			_orderService = orderService;
		}

		public IEnumerable<ReservationServiceTimePeriodDto> ReadTime(int typeId, DateTime? selectdate)
		{
			var model = _roomService.ReadBytypeId(typeId).ToList();
			int maxRoomAmount = model.Count();

			var dtoTime = _TimeRepo.ReadTime().ToList();
			var dto = _orderService.Read();


			if(selectdate != null)
			{
				dto = dto.Where(a => a.AppointmentDate.Date == selectdate.Value.Date);
			}

			var dtoCompare = dto.GroupBy(a => a.AppointmentTimePeriodId)
				.Select(x => new ReservationServiceTimePeriodDto
				{
					Id = x.Key,
					Count = x.Count(),
				});

			foreach(var data in dtoTime)
			{
				var item = dtoCompare.Where(x => x.Id == data.Id).FirstOrDefault();
                if (item != null)
                {
                    data.Count = maxRoomAmount - item.Count;
                }
				else
				{
					data.Count = maxRoomAmount;
				}
            }
			return dtoTime;
		}
	}
}
