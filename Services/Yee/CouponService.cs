using HotelFuen31.APIs.Dtos.Chen;
using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Interfaces.Yee;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace HotelFuen31.APIs.Services.Yee
{
    public class CouponService
    {
        private AppDbContext _db;

        public CouponService(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<CouponTypeDto> GetCouponTypes()
        {
            var couponTypes = _db.CouponTypes
                .AsNoTracking()
                .Select( ct => new CouponTypeDto
                {
                    Id = ct.Id,
                    Name = ct.Name,
                }).ToList();

            return couponTypes;
        }

        public IEnumerable<RoomTypeDtos> GetRoomTypes()
        {
            var roomTypes = _db.RoomTypes
                .AsNoTracking()
                .Select(rt => new RoomTypeDtos
                {
                    RoomTypeId = rt.RoomTypeId,
                    TypeName = rt.TypeName,
                    Description = rt.Description,
                    Capacity = rt.Capacity,
                    BedType = rt.BedType,
                    RoomCount = rt.RoomCount,
                    ImageUrl = rt.ImageUrl,
                    WeekdayPrice = rt.WeekdayPrice,
                    Size = rt.Size,
                }).ToList();

            return roomTypes;
        }

        public IEnumerable<MemberLevelDto> GetMemberLevels()
        {
            var memberLevels = _db.MemberLevels
                .AsNoTracking()
                .Select(ml => new MemberLevelDto
                {
                    Id = ml.Id,
                    Name = ml.Name,
                    LowerSpending = ml.LowerSpending,
                    LowerOrders = ml.LowerOrders,
                    Comment = ml.Comment,
                }).ToList();

            return memberLevels;
        }

        public IEnumerable<IRuleBase> GetUserCoupons(string phone)
        {
            var member = _db.Members
                .AsNoTracking()
                .Include(m => m.CouponMembers)
                    .ThenInclude(cm => cm.Coupon)
                .FirstOrDefault(m => m.Phone == phone);

            if(member == null) throw new Exception("優惠券錯誤: 查無會員");


            var coupons = member.CouponMembers.Select(cm => cm.Coupon);

            if (!coupons.Any()) yield break;

            foreach (var c in coupons)
            {
                switch (c.TypeId)
                {
                    case 1:
                        var couponTD = _db.CouponThresholdDiscounts.FirstOrDefault(crts => crts.CouponId == c.Id);
                        if (couponTD != null)
                        {
                            yield return new TotalPriceDiscountRule(couponTD.CouponId, couponTD.Threshold, couponTD.Discount);
                        }
                        break;

                    case 2:
                        var couponRTS = _db.CouponRoomTimeSpans.FirstOrDefault(crts => crts.CouponId == c.Id);
                        if (couponRTS != null)
                        {
                            yield return new RoomTypeSpanDiscountRule(couponRTS.CouponId, couponRTS.RoomTypeId, couponRTS.StartTime, couponRTS.EndTime, couponRTS.PercentOff, _db);
                        }
                        break;

                    case 3:
                        var couponRCSD = _db.CouponRoomCountSameDates.FirstOrDefault(crts => crts.CouponId == c.Id);
                        if (couponRCSD != null)
                        {
                            yield return new RoomTypeCountDiscountRule(couponRCSD.CouponId, couponRCSD.RoomTypeId, couponRCSD.Count, couponRCSD.PercentOff, _db);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public IEnumerable<IRuleBase> GetAllCoupons()
        {
            var coupons = _db.Coupons.ToList();
            foreach (var c in coupons)
            {
                switch (c.TypeId)
                {
                    case 1:
                        var couponTD = _db.CouponThresholdDiscounts.FirstOrDefault(crts => crts.CouponId == c.Id);
                        if (couponTD != null)
                        {
                            yield return new TotalPriceDiscountRule(couponTD.CouponId, couponTD.Threshold, couponTD.Discount);
                        }
                        break;

                    case 2:
                        var couponRTS = _db.CouponRoomTimeSpans.FirstOrDefault(crts => crts.CouponId == c.Id);
                        if (couponRTS != null)
                        {
                            yield return new RoomTypeSpanDiscountRule(couponRTS.CouponId, couponRTS.RoomTypeId, couponRTS.StartTime, couponRTS.EndTime, couponRTS.PercentOff, _db);
                        }
                        break;

                    case 3:
                        var couponRCSD = _db.CouponRoomCountSameDates.FirstOrDefault(crts => crts.CouponId == c.Id);
                        if (couponRCSD != null)
                        {
                            yield return new RoomTypeCountDiscountRule(couponRCSD.CouponId, couponRCSD.RoomTypeId, couponRCSD.Count, couponRCSD.PercentOff, _db);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public int CreateCoupoon(IFormCollection formData)
        {
            if (!int.TryParse(formData["TypeId"], out int typeId)) throw new Exception("新增優惠券錯誤: 無效的 TypeId");
            if (!DateTime.TryParse(formData["AllowStart"], out DateTime allowStart)) throw new Exception("新增優惠券錯誤: 無效的 AllowStart");
            if (!DateTime.TryParse(formData["AllowEnd"], out DateTime allowEnd)) throw new Exception("新增優惠券錯誤: 無效的 AllowEnd");
            if (!bool.TryParse(formData["Cumulative"], out bool cumulative)) throw new Exception("新增優惠券錯誤: 無效的 Cumulative");
            string? comment = formData["Comment"];

            using(var scope = new TransactionScope())
            {
                var newCoupon = new Coupon
                {
                    TypeId = typeId,
                    AllowStart = allowStart,
                    AllowEnd = allowEnd,
                    Cumulative = cumulative,
                    Comment = comment
                };

                _db.Coupons.Add(newCoupon);
                _db.SaveChanges();

                switch (typeId)
                {
                    case 1:
                        processTD(newCoupon, formData);
                        break;

                    case 2:
                        processRTS(newCoupon, formData);
                        break;

                    case 3:
                        processRCSD(newCoupon, formData);
                        break;

                    default:
                        throw new Exception("新增子優惠券錯誤: 無效的 TypeId");
                }

                _db.SaveChanges();
                scope.Complete();

                return newCoupon.Id;
            }
        }

        public void SendCouponMemberLevel(int memberLevelId, int couponId)
        {
            var members = _db.Members
                .Include(m => m.CouponMembers)
                .Where(m => m.LevelId == memberLevelId)
                .ToList();

            foreach (var member in members)
            {
                member.CouponMembers.Add(new CouponMember
                {
                    MemberId = member.Id,
                    CouponId = couponId,
                });
            }

            _db.SaveChanges();
        }

        private void processTD(Coupon newCoupon, IFormCollection formData)
        {
            if (!int.TryParse(formData["Threshold"], out int threshold)) throw new Exception("新增 TD 優惠券錯誤: 無效的 Threshold");
            if (!int.TryParse(formData["Discount"], out int discount)) throw new Exception("新增 TD 優惠券錯誤: 無效的 Discount");

            var couponTD = new CouponThresholdDiscount
            {
                CouponId = newCoupon.Id,
                Threshold = threshold,
                Discount = discount,
            };

            _db.CouponThresholdDiscounts.Add(couponTD);
        }

        private void processRTS(Coupon newCoupon, IFormCollection formData)
        {
            if (!int.TryParse(formData["RoomTypeId"], out int roomTypeId)) throw new Exception("新增 RTS 優惠券錯誤: 無效的 RoomTypeId");
            if (!DateTime.TryParse(formData["StartTime"], out DateTime startTime)) throw new Exception("新增 RTS 優惠券錯誤: 無效的 StartTime");
            if (!DateTime.TryParse(formData["EndTime"], out DateTime endTime)) throw new Exception("新增 RTS 優惠券錯誤: 無效的 EndTime");
            if (!int.TryParse(formData["Percentoff"], out int percentoff)) throw new Exception("新增 RTS 優惠券錯誤: 無效的 Percentoff");

            var couponRTS = new CouponRoomTimeSpan
            {
                CouponId = newCoupon.Id,
                RoomTypeId = roomTypeId,
                StartTime = startTime,
                EndTime = endTime,
                PercentOff = percentoff,
            };

            _db.CouponRoomTimeSpans.Add(couponRTS);
        }

        private void processRCSD(Coupon newCoupon, IFormCollection formData)
        {
            if (!int.TryParse(formData["RoomTypeId"], out int roomTypeId)) throw new Exception("新增 RCSD 優惠券錯誤: 無效的 RoomTypeId");
            if (!int.TryParse(formData["Count"], out int count)) throw new Exception("新增 RCSD 優惠券錯誤: 無效的 Count");
            if (!int.TryParse(formData["Percentoff"], out int percentoff)) throw new Exception("新增 RCSD 優惠券錯誤: 無效的 Percentoff");

            var couponRCSD = new CouponRoomCountSameDate
            {
                CouponId = newCoupon.Id,
                RoomTypeId = roomTypeId,
                Count = count,
                PercentOff = percentoff,
            };

            _db.CouponRoomCountSameDates.Add(couponRCSD);
        }
    }
}
