using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;
using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Services.FC
{
	public class ReservationServOrderService
	{
		private IReservationServOrderRepo _repo;
		private ReservationServRoomService _roomService;
		public ReservationServOrderService(IReservationServOrderRepo repo, ReservationServRoomService roomService)
		{
			_repo = repo;
			_roomService = roomService;
		}

		public IEnumerable<ReservationServiceOrderDto> Read()
		{
			return _repo.Read();
		}

		public int Create(ReservationVueDto dto)
		{
			//確認服務包廂總數
			var checkmodel = _roomService.ReadBytypeId(1).ToList();
			int maxRoomAmount = checkmodel.Count() - 1; //排除預設包廂

			//確認相同預訂日期與時段數量
			var checkedRoomCount = _repo.ReadByDateTime(dto.AppointmentDate, dto.AppointmentTimePeriodId);
			int totalCount = checkedRoomCount.Count();

			//確認是否有剩餘包廂數量
			if (totalCount < maxRoomAmount)
			{
				var model = new ReservationOrderDto
				{
					ClientId = dto.ClientId,
					ReservationStatusId = 1,
					CreateTime = DateTime.Now,
					ClientName = dto.ClientName,
					PhoneNumber = dto.PhoneNumber,
				};
				int newId = _repo.CreateOrder(model);

				if (newId > 0)
				{
					var model2 = new ReservationServiceOrderDto
					{
						ReservationId = newId,
						ServiceDetailId = dto.ServiceDetailId,
						AppointmentDate = dto.AppointmentDate,
						AppointmentTimePeriodId = dto.AppointmentTimePeriodId,
						TotalDuration = dto.TotalDuration,
						RoomId = 1, //預設值:待安排
						RoomStatusId = 1, //預設值:待安排
						Subtotal = dto.Subtotal
					};
					return _repo.CreateOrderItem(model2);

				}
				return 0;

			}
			else
			{
				throw new Exception("預約失敗~此時段無包廂可使用");
			}
		}
	}
}
