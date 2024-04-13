using HotelFuen31.APIs.Controllers.Yee;
using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace HotelFuen31.APIs.Services.Yee
{
    public class CartRoomService
    {
        private readonly AppDbContext _db;
        public CartRoomService(AppDbContext db)
        {
            _db = db;
        }

        public RoomStockInfo GetRoomStock(string checkInDate, string checkOutDate)
        {
            DateTime startDate;
            DateTime endDate;

            if(!DateTime.TryParse(checkInDate, out startDate) || !DateTime.TryParse(checkOutDate, out endDate)) {
                throw new Exception("日期格式異常");
            }

            // 日期錯置
            if(startDate > endDate)
            {
                DateTime temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            // 查詢日期內剩餘房間
            var list = _db.RoomTypes
                .Include(rt=>rt.Rooms)
                .ThenInclude(r =>r.RoomBookings)
                .ToList()
                .Select(rt => new RoomStokDto
                {
                    Id = rt.RoomTypeId,
                    Name = rt.TypeName,
                    Desc = rt.Description,
                    Capacity = rt.Capacity,
                    BedType = rt.BedType,
                    Price = GetPrice(startDate, endDate, rt.RoomTypeId),
                    WeekdayPrice = rt.WeekdayPrice,
                    WeekendPrice = rt.WeekendPrice,
                    HolidayPrice = rt.HolidayPrice,
                    Picture = rt.ImageUrl,
                    Size = rt.Size,
                    CheckInDate = startDate.ToString("yyyy-MM-dd"),
                    CheckOutDate = endDate.ToString("yyyy-MM-dd"),
                    Info = $"入住時間: {startDate.ToString("yyyy-MM-dd")}, 退房時間: {endDate.ToString("yyyy-MM-dd")}, 備註: 共計 {(endDate - startDate).Days} 日",
                    Rooms = rt.Rooms
                        .Where(r => !r.RoomBookings.Any(rb => (startDate < rb.CheckOutDate && endDate > rb.CheckInDate)))
                        .Select(r => new RoomDto
                        {
                            UId = $"{startDate.ToString("yyyy-MM-dd")},{endDate.ToString("yyyy-MM-dd")},{r.RoomId},{r.RoomTypeId}",
                            RoomId = r.RoomId,
                            TypeId = r.RoomTypeId,
                        }).ToList(),
                }).ToList();

            // 封裝資炫
            var info = new RoomStockInfo
            {
                RequestTime = DateTime.Now,
                CheckInDate = startDate.ToString("yyyy-MM-dd"),
                CheckOutDate = endDate.ToString("yyyy-MM-dd"),
                RoomStocks = list,
            };

            return info;
        }

        public IEnumerable<CartRoomItemDto> CartListUser(string phone)
        {
            string pattern = @"^\d{9,}$"; // 這會匹配長度大於等於 10 的數字

            // 使用正規表達式來驗證 phone
            if (!Regex.IsMatch(phone, pattern)) return Enumerable.Empty<CartRoomItemDto>();

            var dtos = _db.CartRoomItems
                .Include(cri => cri.Type)
                .Where(cri => cri.Phone == phone)
                .ToList()
                .Select(cri => new CartRoomItemDto
                {
                    Id = cri.Id,
                    Uid = cri.Uid,
                    Selected = cri.Selected,
                    TypeId = cri.TypeId,
                    RoomId = cri.RoomId,
                    Name = cri.Type.TypeName,
                    Picture = cri.Type.ImageUrl,
                    CheckInDate = cri.CheckInDate.ToString("yy-MM-dd"),
                    CheckOutDate = cri.CheckOutDate.ToString("yy-MM-dd"),
                    Price = GetPrice(cri.CheckInDate, cri.CheckOutDate, cri.TypeId),
                    Count = 1,
                    Phone = phone,
                    Remark = $"共計 {(cri.CheckOutDate - cri.CheckInDate).Days} 日" + cri.Remark,
                }).ToList();

            return dtos;
        }

        // 合併客戶端及線上購物車
        public void  MergeCart(string phone, IEnumerable<CartRoomItemDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return;

            // 取得現有的購物車項目
            var existingItems = _db.CartRoomItems
                                    .Where(cri => cri.Phone == phone)
                                    .ToList();

            foreach (var dto in dtos)
            {
                // 檢查是否已存在於購物車
                if (!existingItems.Any(cri => cri.Uid == dto.Uid))
                {
                    var strArr = dto.Uid?.Split(",") ?? new string[0];
                    if (strArr.Length >= 3 &&
                        DateTime.TryParse(dto.CheckInDate, out DateTime checkInDate) &&
                        DateTime.TryParse(dto.CheckOutDate, out DateTime checkOutDate) &&
                        int.TryParse(strArr[2], out int roomId))
                    {
                        // 創建新的購物車項目
                        var newItem = new CartRoomItem
                        {
                            Phone = phone,
                            Uid = dto.Uid,
                            Selected = dto.Selected,
                            TypeId = dto.TypeId,
                            RoomId = roomId,
                            CheckInDate = checkInDate,
                            CheckOutDate = checkOutDate,
                            Remark = dto.Remark,
                        };

                        _db.CartRoomItems.Add(newItem);
                    }
                }
            }

            _db.SaveChanges();
        }

        public void DeleteItem(string phone, string uId)
        {
            if (string.IsNullOrEmpty(uId)) throw new Exception("無效 UId");

            var model = _db.CartRoomItems
                .Where(cri => cri.Phone == phone && cri.Uid == uId)
                .FirstOrDefault();

            if (model == null) throw new Exception("查無此項目");

            _db.CartRoomItems.Remove(model);
            _db.SaveChanges();
        }

        public int CreateItem(string phone, CartRoomItemDto dto)
        {
            if (dto == null) return -1;

            var existingItems = _db.CartRoomItems
                                    .Where(cri => cri.Phone == phone)
                                    .ToList();

            if (!existingItems.Any(cri => cri.Uid == dto.Uid))
            {
                var strArr = dto.Uid?.Split(",") ?? new string[0];
                if (strArr.Length >= 3 &&
                    DateTime.TryParse(dto.CheckInDate, out DateTime checkInDate) &&
                    DateTime.TryParse(dto.CheckOutDate, out DateTime checkOutDate) &&
                    int.TryParse(strArr[2], out int roomId))
                {
                    // 創建新的購物車項目
                    var newItem = new CartRoomItem
                    {
                        Phone = phone,
                        Uid = dto.Uid,
                        Selected = dto.Selected,
                        TypeId = dto.TypeId,
                        RoomId = roomId,
                        CheckInDate = checkInDate,
                        CheckOutDate = checkOutDate,
                        Remark = dto.Remark,
                    };

                    _db.CartRoomItems.Add(newItem);
                    _db.SaveChanges();

                    return newItem.Id;
                }
            }

            return -1;
        }

        public void SelectedItem(string phone, CartRoomItemDto dto)
        {
            if (dto == null) throw new Exception("無效傳入");

            var model = _db.CartRoomItems
                .Where(cri => cri.Phone == phone && cri.Uid == dto.Uid)
                .FirstOrDefault();

            if (model == null) throw new Exception("查無此項目");

            model.Selected = dto.Selected;
            _db.SaveChanges();
        }

        public void CheckAll(string phone, bool selected)
        {
            var list = _db.CartRoomItems
                .Where(cri => cri.Phone == phone).ToList();

            if (list == null) throw new Exception("清單為空");

            list.ForEach(model => model.Selected = selected);
            _db.SaveChanges();
        }

        public int GetPrice(string start, string end, int roomTypeId)
        {
            DateTime startDate;
            DateTime endDate;
            if (!DateTime.TryParse(start, out startDate) || !DateTime.TryParse(end, out endDate))
            {
                throw new Exception("字串轉日期異常");
            }

            // 日期錯置
            if (startDate > endDate)
            {
                DateTime temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            // 取得房型
            var roomeType = _db.RoomTypes.Find(roomTypeId);
            if (roomeType == null) throw new Exception("並沒有該房型");


            var query = _db.RoomCalendars
                .AsNoTracking()
                .Where(rc => startDate <= rc.Date && rc.Date < endDate)
                .AsEnumerable();

            var price = query
                .Select(rc => rc.IsHoliday == "true" ?
                    roomeType.HolidayPrice : rc.Week == "五" || rc.Week == "六" || rc.Week == "日" ?
                    roomeType.WeekendPrice : roomeType.WeekdayPrice)
                .Aggregate((total, next) => total + next);

            return price;
        }

        public int GetPrice(DateTime startDate, DateTime endDate, int roomTypeId)
        {
            // 日期錯置
            if (startDate > endDate)
            {
                DateTime temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            var roomeType = _db.RoomTypes.Find(roomTypeId);
            if (roomeType == null) throw new Exception("並沒有該房型");

            var query = _db.RoomCalendars
                .AsNoTracking()
                .Where(rc => startDate <= rc.Date && rc.Date < endDate)
                .AsEnumerable();

            var price = query
                .Sum(rc => rc.IsHoliday == "true" ?
                    roomeType.HolidayPrice : rc.Week == "五" || rc.Week == "六" || rc.Week == "日" ?
                    roomeType.WeekendPrice : roomeType.WeekdayPrice);

            return price;
        }
    }
}
