using HotelFuen31.APIs.Interfaces.Yee;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class RoomTypeCountDiscountRule : IRuleBase
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Note { get; set; }

        private AppDbContext _db;

        private RoomType _type;
        private int _count;
        private int _percentOff;

        public RoomTypeCountDiscountRule(int id, int roomTypeId, int count, int percentOff, AppDbContext db)
        {
            this.Id = id;
            _count = count;
            _percentOff = percentOff;
            _db = db;

            var type = _db.RoomTypes.Find(roomTypeId);
            if (type == null) throw new Exception("優惠券建立錯誤：並無該房型；RoomTypeSpanDiscountRule");

            _type = type;

            int showPercentOff = (100 - _percentOff) % 10 == 0 ? (100 - _percentOff) / 10 : 100 - _percentOff;
            this.Name = this.Name = $"{type.TypeName} 當日第 {count} 間只要 {showPercentOff} 折!";
            this.Note = "多件優惠";
        }

        public IEnumerable<Discount> Process(CartContext cart)
        {
            // 建立日期及符合商品之字典
            Dictionary<DateTime, List<RoomPrePayDto>> dicDateCount = new Dictionary<DateTime, List<RoomPrePayDto>>();

            foreach (var p in cart.PurchasedItems)
            {
                if (!(p is RoomPrePayDto rppd) || rppd.RoomTypeId != _type.RoomTypeId) continue;

                DateTime flag = rppd.CheckInDate.Date;
                DateTime checkOutDate = rppd.CheckOutDate.Date;

                while (flag < checkOutDate)
                {
                    if (!dicDateCount.ContainsKey(flag)) dicDateCount.Add(flag, new List<RoomPrePayDto>());

                    dicDateCount[flag].Add(rppd);

                    flag = flag.AddDays(1);
                }
            }

            // 篩出該日期超過 _count 的字典
            var dicFiltered = dicDateCount.Where(kvp => kvp.Value.Count / _count >= 1).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // 由於 EF LINQ 無法識別 Dictionary.ContainsKey，先轉換為 List
            var listFiltered = dicFiltered.Keys.ToList();

            // 計算總折扣
            var amount = _db.RoomCalendars
                .AsNoTracking()
                .Where(rc => listFiltered.Contains(rc.Date))
                .ToList()
                .Sum(rc =>
                {
                    int price = rc.IsHoliday == "true" ?
                                        _type.HolidayPrice : rc.Week == "五" || rc.Week == "六" || rc.Week == "日" ?
                                        _type.WeekendPrice : _type.WeekdayPrice;

                    int group = dicDateCount[rc.Date].Count / _count;
                    return price * _percentOff / 100 * group;
                });


            yield return new Discount()
            {
                Amount = amount,
                Products = dicFiltered.SelectMany(kvp => kvp.Value).Distinct().ToArray(),
                Rule = this,
            };
        }
    }
}
