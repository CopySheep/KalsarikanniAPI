namespace HotelFuen31.APIs.Dtos.FC
{
	public class ReservationServiceTimePeriodDto
	{
		public int Id { get; set; }

		//可服務時段
		public string TimePeriod { get; set; }

		//空房剩餘數量
		public int Count { get; set; }

	}
}
