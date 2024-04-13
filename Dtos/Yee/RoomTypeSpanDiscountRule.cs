using HotelFuen31.APIs.Interfaces.Yee;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class RoomTypeSpanDiscountRule : IRuleBase
    {
        public int Id { get;}
        public string Name { get; set; }
        public string Note { get; set; }

        private AppDbContext _db;

        private RoomType _type;
        private DateTime _start;
        private DateTime _end;
        private int _percentOff;

        public RoomTypeSpanDiscountRule(int id, int roomTypeId, DateTime start, DateTime end, int percentOff, AppDbContext db)
        {
            if (start > end) throw new Exception("優惠券建立錯誤：日期錯置；RoomTypeSpanDiscountRule");

            this.Id = id;
            _db = db;
            _start = start;
            _end = end;
            _percentOff = percentOff;

            var type = _db.RoomTypes.Find(roomTypeId);
            if (type == null) throw new Exception("優惠券建立錯誤：並無該房型；RoomTypeSpanDiscountRule");
            _type = type;

            int showPercentOff = (100 - _percentOff) % 10 == 0 ? (100 - _percentOff) / 10 : 100 - _percentOff;
            this.Name = this.Name = $"{type.TypeName} {_start.ToString("yyyy/MM/dd")} ~ {_end.ToString("yyyy/MM/dd")} 只要 {showPercentOff} 折!";
            this.Note = "限時優惠";
        }

        public IEnumerable<Discount> Process(CartContext cart)
        {
            // 建立日期字典
            Dictionary<DateTime, List<RoomPrePayDto>> dicMached = new Dictionary<DateTime, List<RoomPrePayDto>>();

            foreach (var p in cart.PurchasedItems)
            {
                if (!(p is RoomPrePayDto rppd) || rppd.RoomTypeId != _type.RoomTypeId) continue;

                DateTime checkInDate = rppd.CheckInDate.Date;
                DateTime checkOutDate = rppd.CheckOutDate.Date;

                // 若在區間內填寫字典
                if (checkInDate <= _end && checkOutDate > _start)
                {
                    var flag = checkInDate > _start ? checkInDate : _start;

                    // 4/3 結束活動代表 4/4 以前有效
                    var endLine = checkOutDate < _end.AddDays(1) ? checkOutDate : _end.AddDays(1);

                    while (flag < endLine)
                    {
                        // 判斷有無該 key
                        if (!dicMached.ContainsKey(flag)) dicMached.Add(flag, new List<RoomPrePayDto>());
                        dicMached[flag].Add(rppd);

                        flag = flag.AddDays(1);
                    }
                }
            }

            var listMached = dicMached.Select(kvp => kvp.Key).ToList();

            // 加總所有折扣金額
            var amount = _db.RoomCalendars
                .AsNoTracking()
                .Where(rc => listMached.Contains(rc.Date))
                .ToList()
                .Sum(rc =>
                {
                    int price = rc.IsHoliday == "true" ?
                            _type.HolidayPrice : rc.Week == "五" || rc.Week == "六" || rc.Week == "日" ?
                            _type.WeekendPrice : _type.WeekdayPrice;


                    int rooms = dicMached[rc.Date].Count;
                    return price * _percentOff / 100 * rooms;
                });

            yield return new Discount()
            {
                Amount = amount,
                Products = dicMached.SelectMany(kvp => kvp.Value).Distinct().ToArray(),
                Rule = this,
            };
        }
    }
}
