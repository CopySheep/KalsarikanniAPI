using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Interfaces.Yee;
using HotelFuen31.APIs.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelFuen31.APIs.Services.Yee
{
    public class PreOrderService
    {
        private readonly AppDbContext _db;
        public PreOrderService(AppDbContext db)
        {
            _db = db;
        }

        public PreOrderDto GetUserBudget(string phone)
        {
            if (!_db.Members.Any(m => m.Phone == phone)) throw new Exception("取得預節錯誤: 查無該用戶");

            CartContext cart = new CartContext();
            POS pos = new POS();

            cart.PurchasedItems.AddRange(LoadProducts(phone));
            pos.ActivedRules.AddRange(LoadRules(phone));

            pos.CheckoutProcess(cart);

            var preOrder = new PreOrderDto
            {

                PurchasedItems = cart.PurchasedItems,
                AppliedDiscounts = cart.AppliedDiscounts.Select(ad => new DiscountDto
                {
                    Id = ad.Id,
                    Rule = ad.Rule?.RuleToDto(),
                    MachedIndex = ad.Products?.Select(p => p.Index).ToList(),
                    Amount = ad.Amount,
                }).ToList(),
            };

            return preOrder;
        }

        private IEnumerable<IPrePayDto> LoadProducts(string phone)
        {
            var roomCalendars = _db.RoomCalendars.AsNoTracking().ToList();

            var items = _db.CartRoomItems
                .Include(cri => cri.Type)
                .Where(cri => cri.Phone == phone && cri.Selected)
                .ToList()
                .Select((cri, index) =>
                {
                    var totalPrice = roomCalendars
                        .Where(rc => cri.CheckInDate <= rc.Date && rc.Date < cri.CheckOutDate)
                        .Sum(rc => rc.IsHoliday == "true" ?
                                       cri.Type.HolidayPrice : rc.Week == "五" || rc.Week == "六" || rc.Week == "日" ?
                                       cri.Type.WeekendPrice : cri.Type.WeekdayPrice);

                    return new RoomPrePayDto
                    {
                        Index = index + 1,
                        Price = totalPrice,
                        SKU = cri.Uid,
                        Name = cri.Type.TypeName,
                        Info = $"{cri.CheckInDate.ToString("yyyy/MM/dd")}~{cri.CheckOutDate.ToString("yyyy/MM/dd")},{cri.Remark}",
                        RoomTypeId = cri.TypeId,
                        CheckInDate = cri.CheckInDate,
                        CheckOutDate = cri.CheckOutDate,
                    };
                }).ToList();

            if (items.Count < 1) throw new Exception("取得預節錯誤: 查無結算項目");

            return items;
        }

        private IEnumerable<IRuleBase> LoadRules(string phone)
        {
            var member = _db.Members
                .Include(m => m.CouponMembers)
                .ThenInclude(cm => cm.Coupon)
                .FirstOrDefault(m => m.Phone == phone);
            if (member == null) throw new Exception("取得預節錯誤: 查無該用戶");

            foreach(var cm in member.CouponMembers)
            {
                switch (cm.Coupon.TypeId) {
                    case 1:
                        var couponTD = _db.CouponThresholdDiscounts.FirstOrDefault(crts => crts.CouponId == cm.Coupon.Id);
                        if (couponTD != null)
                        {
                            yield return new TotalPriceDiscountRule(couponTD.CouponId, couponTD.Threshold, couponTD.Discount);
                        }
                        break;

                    case 2:
                        var couponRTS = _db.CouponRoomTimeSpans.FirstOrDefault(crts => crts.CouponId == cm.Coupon.Id);
                        if (couponRTS != null) {
                            yield return new RoomTypeSpanDiscountRule(couponRTS.CouponId, couponRTS.RoomTypeId, couponRTS.StartTime, couponRTS.EndTime, couponRTS.PercentOff, _db);
                        }
                        break;

                    case 3:
                        var couponRCSD = _db.CouponRoomCountSameDates.FirstOrDefault(crts => crts.CouponId == cm.Coupon.Id);
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

    }

    public static class RuleExts
    {
        public static RuleDto RuleToDto(this IRuleBase rule)
        {
            var dto = new RuleDto
            {
               Id = rule.Id,
               Name = rule.Name,
               Note = rule.Note,
            };

            return dto;
        }
    }

    //public static class CartRoomItemsExt
    //{
    //    public static IPrePayDto ToPrePay(this IEnumerable<CartRoomItem> items)
    //    {
    //        var dto = items.Select((cri, index) => new RoomPrePayDto
    //        {
    //            Index = index + 1,
    //            Price = _db.RoomCalendars
    //                    .AsNoTracking()
    //            .Where(rc => cri.CheckInDate <= rc.Date && rc.Date < cri.CheckOutDate)
    //            .ToList()
    //                    .Sum(rc => rc.IsHoliday == "true" ?
    //                    cri.Type.HolidayPrice : rc.Week == "五" || rc.Week == "六" || rc.Week == "日" ?
    //                                    cri.Type.WeekendPrice : cri.Type.WeekdayPrice),
    //            SKU = cri.Uid,
    //            Name = cri.Type.TypeName,
    //            Info = $"{cri.CheckInDate.ToString("yyyy/MM/dd")}~{cri.CheckOutDate.ToString("yyyy/MM/dd")},{cri.Remark}",
    //            RoomTypeId = cri.TypeId,
    //            CheckInDate = cri.CheckInDate,
    //            CheckOutDate = cri.CheckOutDate,
    //        });
    //    }
    //}
}
