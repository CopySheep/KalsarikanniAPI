using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Haku
{
	public class CarTaxiOrderItemDto
	{
		public int Id { get; set; }

		public int CarId { get; set; }

		public string PickUpLongtitude { get; set; }

		public string PickUpLatitude { get; set; }

		public string PickUpLocation { get; set; }

		public string DestinationLatitude { get; set; }

		public string DestinationLongtitude { get; set; }

		public string DestinationLocation { get; set; }

		public decimal Total { get; set; }

		public string StartTime { get; set; }

		public string EndTime { get; set; }

		public int EmpId { get; set; }

		public int MemberId { get; set; }

		public CarsDto? Car {  get; set; } 
	}
}
