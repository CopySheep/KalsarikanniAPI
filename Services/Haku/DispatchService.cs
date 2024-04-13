using HotelFuen31.APIs.Dtos.Yee;
using System.Text.RegularExpressions;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;
using HotelFuen31.APIs.Dtos.Haku;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HotelFuen31.APIs.Services.Haku
{
	public class DispatchService
	{
		private readonly AppDbContext _db;
		public DispatchService(AppDbContext db)
		{
			_db = db;
		}

		//找到那個會員的訂單細項資料
		public IEnumerable<CarTaxiOrderItemDto> OrderListUser(string phone)
		{
			string pattern = @"^\d{9,}$"; // 這會匹配長度大於等於 10 的數字

			//使用正規表達式來驗證 phone
			if (!Regex.IsMatch(phone, pattern)) return Enumerable.Empty<CarTaxiOrderItemDto>();

			int memId = _db.Members
				.Where(m => m.Phone == phone)
				.Select(m => m.Id)
				.FirstOrDefault();

			//int memId = 1;// 測試用

			var dtos = _db.CarTaxiOrderItems
				.Include(ctoi => ctoi.Member)
				.Include(ctoi=>ctoi.Car)
				.Where(ctoi => ctoi.MemberId == memId)
				.ToList()
				.Select(ctoi => new CarTaxiOrderItemDto
				{
					Id = ctoi.Id,
					CarId = ctoi.CarId,
					PickUpLongtitude = ctoi.PickUpLongtitude,
					PickUpLatitude = ctoi.PickUpLatitude,
					PickUpLocation = ctoi.PickUpLocation,
					DestinationLatitude = ctoi.DestinationLatitude,
					DestinationLongtitude = ctoi.DestinationLongtitude,
					DestinationLocation = ctoi.DestinationLocation,
					Total = ctoi.Total,
					StartTime = ctoi.StartTime.ToString(),
					EndTime = ctoi.EndTime.ToString(),
					EmpId = ctoi.EmpId,
					MemberId = ctoi.MemberId,
					Car=new CarsDto
					{
						Id=ctoi.Car.Id,
						EmpId = ctoi.Car.EmpId,
						Capacity = ctoi.Car.Capacity,
						PlusPrice = ctoi.Car.PlusPrice,
						Comment = ctoi.Car.Comment,
						Picture = ctoi.Car.Picture,
						Description = ctoi.Car.Description,
					}
				})
				.ToList();	

			return dtos;
		}

		//查剩下的車車有哪些
		public IEnumerable<CarsDto> RemainingCars(CarTaxiOrderItemDto dto)
		{
			if (!DateTime.TryParse(dto.StartTime, out DateTime start) || !DateTime.TryParse(dto.EndTime, out DateTime end))
			{
				return Enumerable.Empty<CarsDto>();
			}


			//var begin = new DateTime(start.Year, start.Month, start.Day);
			//var finish = new DateTime(end.Year, end.Month, end.Day + 1);

			// 當天沒有任何行程的車
			//var cars = unConflictCars(begin, finish);
			//if (cars.Count() > 0) return cars;



			// 找出在指定時間範圍內沒有訂單的車輛
			var cars = unConflictCars(start, end);
			return cars;
		}

		private IEnumerable<CarsDto> unConflictCars(DateTime start, DateTime end)
		{
			//var now = DateTime.Now;
			var cars = _db.Cars
				.Include(c => c.CarTaxiOrderItems)
				.Where(c => !c.CarTaxiOrderItems
								.Any(ctoi => ctoi.StartTime < end && ctoi.EndTime > start) /*|| c.CarTaxiOrderItems.Count == 0*/)
				.ToList()
				.Select(c => new CarsDto
				{
					Id = c.Id,
					EmpId = c.EmpId,
					Capacity = c.Capacity,
					PlusPrice = c.PlusPrice,
					Comment = c.Comment,
					Picture = c.Picture,
					Description = c.Description,
					ASC = c.CarTaxiOrderItems
						.OrderBy(ctoi => ctoi.StartTime)
						.FirstOrDefault(ctoi => ctoi.StartTime > end)
						?.EfToDto(),
					DESC = c.CarTaxiOrderItems
						.OrderByDescending(ctoi => ctoi.EndTime)
						.FirstOrDefault(ctoi => ctoi.EndTime < start)
						?.EfToDto(),
				}).ToList();
			return cars;
		}

		//新增搭乘訂單細項
		public int CreateItem(string phone, CarTaxiOrderItemDto dto)
		{
			if (dto == null) return -1;

			int memId = _db.Members
				.Where(m => m.Phone == phone)
				.Select(m => m.Id)
				.FirstOrDefault();

			// 測試用
			//int memId = 1;
			// 測試用

			int empId = _db.Cars.Where(c => c.Id == dto.CarId).Select(c=>c.EmpId).FirstOrDefault();

				if (DateTime.TryParse(dto.StartTime, out DateTime start) && DateTime.TryParse(dto.EndTime, out DateTime end))
			{
				//檢查是否有重複訂單
				if (_db.CarTaxiOrderItems.Any(c => c.StartTime < end && c.EndTime > start && c.CarId==dto.CarId)) return -1;
				else
				{
					// 創建新的購物車項目
					var newItem = dto.DtoToEf(start, end, empId, memId);

						_db.CarTaxiOrderItems.Add(newItem);
					_db.SaveChanges();


					return newItem.Id;
				}
			}

			return -1;
		}

		public string CarOrderDelete(int id)
		{
			var carOrder = _db.CarTaxiOrderItems.Find(id);
			if (carOrder == null)
			{
				return "沒有此訂單項目";
			}
			_db.CarTaxiOrderItems.Remove(carOrder); 
			_db.SaveChanges();
			return "刪除成功";
		}
	}
}
	public static class CarTaxiOrderItemExts { 
		public static CarTaxiOrderItemDto EfToDto(this CarTaxiOrderItem ctoi) {

			return new CarTaxiOrderItemDto
			{
				Id = ctoi.Id,
				CarId = ctoi.CarId,
				PickUpLongtitude = ctoi.PickUpLongtitude,
				PickUpLatitude = ctoi.PickUpLatitude,
				PickUpLocation = ctoi.PickUpLocation,
				DestinationLatitude = ctoi.DestinationLatitude,
				DestinationLongtitude = ctoi.DestinationLongtitude,
				DestinationLocation= ctoi.DestinationLocation,
				Total = ctoi.Total,
				StartTime = ctoi.StartTime.ToString(),
				EndTime = ctoi.EndTime.ToString(),
				EmpId = ctoi.EmpId,
				MemberId = ctoi.MemberId,
			};
		}

		public static CarTaxiOrderItem DtoToEf(this CarTaxiOrderItemDto dto, DateTime start, DateTime end, int empId, int memId)
		{
			return new CarTaxiOrderItem
			{
				CarId = dto.CarId,

				PickUpLongtitude = dto.PickUpLongtitude,

				PickUpLatitude = dto.PickUpLatitude,

				PickUpLocation= dto.PickUpLocation,

				DestinationLatitude = dto.DestinationLatitude,

				DestinationLongtitude = dto.DestinationLongtitude,

				DestinationLocation= dto.DestinationLocation,

				Total = dto.Total,

				StartTime = start,

				EndTime = end,

				EmpId = empId,

				MemberId = memId
			};
		}
	}


