using Hangfire.Server;
using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Interfaces.Yee;
using HotelFuen31.APIs.Models;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Transactions;
using System.Web;
using static NuGet.Packaging.PackagingConstants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelFuen31.APIs.Services.Yee
{
    public class OrderService
    {
        private readonly AppDbContext _db;
        private readonly CartRoomService _crService;
        private readonly PreOrderService _poService;

        public OrderService(AppDbContext db, CartRoomService crService, PreOrderService poService)
        {
            _db = db;
            _crService = crService;
            _poService = poService;
        }

        public IEnumerable<OrderDto> GetUserOrders(string phone)
        {
            var orders = _db.Orders
                .Include(o => o.RoomBookings)
                .ThenInclude(rb => rb.Room)
                .ThenInclude(r => r.RoomType)
                .Where(o => o.Phone == phone)
                .ToList();

            if (!orders.Any()) return Enumerable.Empty<OrderDto>();

            var dtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                Phone = o.Phone,
                Status = o.Status,
                OrderTime = o.OrderTime,
                MerchantTradeNo = o.MerchantTradeNo,
                RtnCode = o.RtnCode,
                RtnMsg = o.RtnMsg,
                TradeNo = o.TradeNo,
                //TradeAmt = o.RoomBookings.Count > 0 ? o.RoomBookings.Sum(rb => rb.OrderPrice) : 0,
                TradeAmt = o.TradeAmt,
                PaymentDate = o.PaymentDate,
                PaymentType = o.PaymentType,
                PaymentTypeChargeFee = o.PaymentTypeChargeFee,
                TradeDate = o.TradeDate,
                SimulatePaid = o.SimulatePaid,
                RoomBookings = o.RoomBookings != null ? o.RoomBookings.Select(rb => new RoomBookingDto
                {
                    BookingId = rb.BookingId,
                    OrderId = rb.OrderId,
                    RoomId = rb.RoomId,
                    CheckInDate = rb.CheckInDate,
                    CheckOutDate = rb.CheckOutDate,
                    MemberId = rb.MemberId,
                    Remark = rb.Remark,
                    BookingStatusId = rb.BookingStatusId,
                    BookingCancelDate = rb.BookingCancelDate,
                    BookingDate = rb.BookingDate,
                    OrderPrice = rb.OrderPrice,
                    Phone = rb.Phone,
                    RoomTypeId = rb.Room.RoomTypeId,
                    Name = rb.Room.RoomType.TypeName,
                }).ToList() : Enumerable.Empty<RoomBookingDto>(),
            });

            return dtos;
        }

        public int CreateOrder(string phone, int[] useCoupons)
        {
            //var selectedItem = _db.CartRoomItems.Where(cri => cri.Phone == phone && cri.Selected);
            //if (!selectedItem.Any()) throw new Exception("訂單項目為空");
           
            var member = _db.Members
                            .AsNoTracking()
                            .Include(m => m.CouponMembers)
                                .ThenInclude(mc => mc.Coupon)
                            .Select( m => new MemberCheckOutDto
                            {
                                Id = m.Id,
                                Phone = m.Phone,
                                Selected = _db.CartRoomItems
                                    .Include(cri => cri.Type)
                                    .Where(cri => cri.Phone == phone && cri.Selected)
                                    .ToList(),
                                CouponMembers = m.CouponMembers.ToList(),
                            })
                            .FirstOrDefault(m => m.Phone == phone);

            if (member == null) throw new Exception("建立訂單錯誤: 查無該會員");
            if (member.Selected == null || member.Selected.Count() < 1) throw new Exception("建立訂單錯誤: 訂單項目為空");
            //if (!useCoupons.Except(member.CouponMembers.Select(c => c.Coupon.Id)).Any()) throw new Exception("建立訂單錯誤: 會員並無該優惠券");

            var selectedItem = member.Selected.ToList();
            var actualCoupons = member.CouponMembers
                ?.Where(cm => useCoupons.Contains(cm.Coupon.Id))
                .GroupBy(cm => cm.CouponId) // 按 CouponId 分组
                .Select(group => group.First()) // 选择每个组中的第一个元素
                .ToList() ?? new List<CouponMember>();

            var preOrder = GetCheckout(selectedItem, actualCoupons);

            if (!int.TryParse(preOrder.FinalPrice.ToString(), out int final)) throw new Exception("建立訂單錯誤: decimal 轉型錯誤");

            // todo 有效期限

            using (var scope = new TransactionScope())
            {
                // 建立訂單
                var order = new Order
                {
                    Phone = phone,
                    Status = 0,
                    OrderTime = DateTime.Now,
                };
                _db.Orders.Add(order);
                _db.SaveChanges();

                //EcPay
                // order.MerchantTradeNo = MTNBulder(order.Id);    // 綠界廠商交易編號，由訂單編號和交易編號(亂數)組成
                order.RtnCode = 0;  // 未付款
                order.RtnMsg = "訂單已建立，尚未付款";
                //order.TradeNo = order.MerchantTradeNo.Substring(11, 20);   // ?交易編號(亂數)
                //order.TradeAmt = 0
                //order.PaymentDate = order.OrderTime;
                order.PaymentType = "aio";
                order.PaymentTypeChargeFee = "0";
                //order.TradeDate = EcPayDto.MerchantTradeDate;
                //order.SimulatePaid = 0;     // 模擬付款
                _db.SaveChanges();


                // 逐一寫入
                selectedItem.ForEach(item =>
                {
                    // 確認有無空房
                    var room = _db.Rooms
                                   .Include(r => r.RoomBookings)
                                   .Where(r => r.RoomTypeId == item.TypeId)
                                   .FirstOrDefault(r => !r.RoomBookings.Any(rb => (item.CheckInDate < rb.CheckOutDate && item.CheckOutDate > rb.CheckInDate)));

                    if (room == null) throw new Exception("建立訂單錯誤: 已無空房");

                    // 新建訂單項目
                    var newBooking = new RoomBooking
                    {
                        RoomId = room.RoomId,
                        OrderId = order.Id,
                        CheckInDate = item.CheckInDate,
                        CheckOutDate = item.CheckOutDate,
                        MemberId = _db.Members.FirstOrDefault(m => m.Phone == phone)?.Id ?? null,
                        Remark = item.Remark,
                        BookingStatusId = 2,
                        BookingDate = order.OrderTime,
                        OrderPrice = _crService.GetPrice(item.CheckInDate, item.CheckOutDate, item.TypeId),
                        Phone = phone,
                    };

                    _db.RoomBookings.Add(newBooking);
                    _db.SaveChanges();
                });


                if(actualCoupons?.Count() > 0)
                {
                    // 排除已提交優惠券
                    _db.CouponMembers.RemoveRange(actualCoupons);
                }

                // 購物車排除已提交訂單
                var removeItems = _db.CartRoomItems.Where(cri => cri.Phone == phone && cri.Selected);
                _db.CartRoomItems.RemoveRange(removeItems);

                order.TradeAmt = final;

                _db.SaveChanges();

                // 提交
                scope.Complete();

                return order.Id;
            }
        }

        public OrderDto GetOrder(string phone, int orderId)
        {
            var order = _db.Orders
                .Include(o => o.RoomBookings)
                .ThenInclude(rb => rb.Room)
                .ThenInclude(r => r.RoomType)
                .FirstOrDefault(o => o.Phone == phone && o.Id == orderId);

            if (order == null) throw new Exception("查無該筆訂單");

            var dto = new OrderDto
            {
                Id = order.Id,
                Phone = order.Phone,
                Status = order.Status,
                OrderTime = order.OrderTime,
                MerchantTradeNo = order.MerchantTradeNo,
                RtnCode = order.RtnCode,
                RtnMsg = order.RtnMsg,
                TradeNo = order.TradeNo,
                //TradeAmt = order.TradeAmt = order.RoomBookings?.Sum(rb => rb.OrderPrice),
                TradeAmt = order.TradeAmt,
                PaymentDate = order.PaymentDate,
                PaymentType = order.PaymentType,
                PaymentTypeChargeFee = order.PaymentTypeChargeFee,
                TradeDate = order.TradeDate,
                SimulatePaid = order.SimulatePaid,
                RoomBookings = order.RoomBookings?.Select(rb => new RoomBookingDto
                {
                    BookingId = rb.BookingId,
                    OrderId = rb.OrderId,
                    RoomId = rb.RoomId,
                    CheckInDate = rb.CheckInDate,
                    CheckOutDate = rb.CheckOutDate,
                    MemberId = rb.MemberId,
                    Remark = rb.Remark,
                    BookingStatusId = rb.BookingStatusId,
                    BookingCancelDate = rb.BookingCancelDate,
                    BookingDate = rb.BookingDate,
                    OrderPrice = rb.OrderPrice,
                    Phone = rb.Phone,
                    RoomTypeId = rb.Room.RoomTypeId,
                    Name = rb.Room.RoomType.TypeName,
                }).ToList() ?? Enumerable.Empty<RoomBookingDto>(),
            };

            return dto;
        }

        public Dictionary<string, string> GetECPayDic(OrderDto orderDto, string backEnd, string frontEnd)
        {
            if (orderDto.Status == 1 || orderDto.RtnCode == 1) throw new Exception("該訂單已完成付款");

            string merchantTradeNo = MTNBulder(orderDto.Id);

            // 編寫字典以利加密(綠界要求)
            var dicOrder = new Dictionary<string, string>
            {
                { "MerchantID", "3002607" },
                { "MerchantTradeNo", merchantTradeNo },
                { "MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                { "PaymentType", "aio"},
                { "TotalAmount", orderDto.TradeAmt?.ToString() ?? "0" },
                { "TradeDesc", "Kalsari Hotel" },   // todo 交易描述
                { "ItemName",  "Kalsari Hotel Booking" },
                { "ReturnURL",  $"{backEnd}/api/Order/ECPay"},  // 綠界 POST 回後端
                { "ChoosePayment", "ALL" },
                { "EncryptType",  "1" },
                { "ClientBackURL", $"{frontEnd}" },    // 僅為綠界端轉址返回按鈕，並無 POST 功能
                { "OrderResultURL", $"{frontEnd}/Pay?orderId={orderDto.Id}" },   // 綠界 POST 回前端
            };

            // 透過字典(綠界要求)
            dicOrder["CheckMacValue"] = GetCheckMacValue(dicOrder);


            // 填入資料庫以利檢查
            var order = _db.Orders.Find(orderDto.Id);

            if (order == null) throw new Exception("訂單比對異常");

            order.MerchantTradeNo = merchantTradeNo;
            order.TradeNo = merchantTradeNo.Substring(10, 10);
            order.TradeDate = dicOrder["MerchantTradeDate"];
            order.SimulatePaid = 1;
            order.CheckMacValue = dicOrder["CheckMacValue"];

            _db.SaveChanges();

            return dicOrder;
        }

        public int UpdateECpay(Dictionary<string, string> dictionary)
        {
            var checkvalue = GetCheckMacValue(dictionary);
            var checkMacValue = dictionary["CheckMacValue"];

            if (checkvalue != checkMacValue) throw new Exception("校驗錯誤");

            var order = _db.Orders.Where(o => o.MerchantTradeNo == dictionary["MerchantTradeNo"]).FirstOrDefault();

            if (order == null) throw new Exception("查無該筆訂單");
           
            int rtnCode = int.Parse(dictionary["RtnCode"]);
            //string rtnMsg = dictionary["RtnMsg"];
            //string tradeNo = dictionary["TradeNo"];
            //int tradeAmt = int.Parse(dictionary["TradeAmt"]);
            //DateTime paymentDate = Convert.ToDateTime(dictionary["PaymentDate"]);
            //string paymentType = dictionary["PaymentType"];
            //string paymentTypeChargeFee = dictionary["PaymentTypeChargeFee"];
            //string tradeDate = dictionary["TradeDate"];
            //int simulatePaid = int.Parse(dictionary["SimulatePaid"]);

            if (rtnCode == 1)
            {
                order.RtnCode = rtnCode;
                order.Status = rtnCode;
                order.RtnMsg = "付款成功，期待您的光臨";
                order.TradeNo = dictionary["TradeNo"];
                order.TradeAmt = int.Parse(dictionary["TradeAmt"]);
                order.PaymentDate = Convert.ToDateTime(dictionary["PaymentDate"]);
                order.PaymentType = dictionary["PaymentType"];
                order.PaymentTypeChargeFee = dictionary["PaymentTypeChargeFee"];
                order.TradeDate = dictionary["TradeDate"];
                order.SimulatePaid = int.Parse(dictionary["SimulatePaid"]);
                order.CheckMacValue = checkMacValue;
            }

            _db.SaveChanges();

            return order.Id;
        }

        public string GetCheckMacValue(Dictionary<string, string> dictionary)
        {
            string merchantKey = "pwFHCqoQZGmho4w6";
            string merchantIv = "EkRm7iFT261dpevs";

            string raw = $"HashKey={merchantKey}&" +
                         string.Join("&", dictionary.Where(x => x.Key != "CheckMacValue")
                                                    .OrderBy(x => x.Key)
                                                    .Select(p => $"{p.Key}={p.Value}")) +
                         $"&HashIV={merchantIv}";

            string encoded = HttpUtility.UrlEncode(raw).ToLower();

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(encoded));
                string checkMacValue = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
                return checkMacValue;
            }
        }

        // 產生綠界交易碼
        private string MTNBulder(int orderId)
        {
            return $"{new string('0', 10 - orderId.ToString().Length)}{orderId}{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10)}";
        }

        private IEnumerable<IPrePayDto> LoadProducts(IEnumerable<CartRoomItem> items)
        {
            var checkInDates = items.Select(i => i.CheckInDate);
            var checkOutDates = items.Select(i => i.CheckOutDate);
            var minDate = checkInDates.Min();
            var maxDate = checkOutDates.Max();

            var relevantRoomCalendars = _db.RoomCalendars
                .AsNoTracking()
                .Where(rc => rc.Date >= minDate && rc.Date < maxDate)
                .ToList();

            var dtos = items.Select((cri, index) =>
            {
                var price = relevantRoomCalendars
                    .Where(rc => cri.CheckInDate <= rc.Date && rc.Date < cri.CheckOutDate)
                    .Sum(rc => rc.IsHoliday == "true" ?
                                    cri.Type.HolidayPrice : rc.Week == "五" || rc.Week == "六" || rc.Week == "日" ?
                                    cri.Type.WeekendPrice : cri.Type.WeekdayPrice);

                return new RoomPrePayDto
                {
                    Index = index + 1,
                    Price = price,
                    SKU = cri.Uid,
                    Name = cri.Type.TypeName,
                    Info = $"{cri.CheckInDate:yyyy/MM/dd}~{cri.CheckOutDate:yyyy/MM/dd},{cri.Remark}",
                    RoomTypeId = cri.TypeId,
                    CheckInDate = cri.CheckInDate,
                    CheckOutDate = cri.CheckOutDate,
                };
            }).ToList();

            if (dtos.Count < 1) throw new Exception("新增訂單錯誤: 查無結算項目");

            return dtos;
        }

        private IEnumerable<IRuleBase> LoadRules(IEnumerable<Coupon> coupons)
        {
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

        public PreOrderDto GetCheckout(IEnumerable<CartRoomItem> selectedItem, IEnumerable<CouponMember> actualCoupons)
        {
            CartContext cart = new CartContext();
            POS pos = new POS();

            var prePayDtos = LoadProducts(selectedItem);
            var rules = LoadRules(actualCoupons?.Select(cm => cm.Coupon).ToList() ?? new List<Coupon>());

            cart.PurchasedItems.AddRange(prePayDtos);
            pos.ActivedRules.AddRange(rules);

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

        //private string GetCheckMacValue(Dictionary<string, string> dicOrder)
        //{
        //    var param = dicOrder.Keys.OrderBy(x => x).Select(key => key + "=" + dicOrder[key]).ToList();
        //    string checkValue = string.Join("&", param);

        //    //綠界提供測試用的 HashKey 真實註冊會有另一組
        //    var hashKey = "pwFHCqoQZGmho4w6";
        //    //綠界提供測試用的 HashIV 真實註冊會有另一組
        //    var hashIV = "EkRm7iFT261dpevs";

        //    checkValue = $"HashKey={hashKey}&" + checkValue + $"&HashIV={hashIV}";
        //    checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
        //    checkValue = GetSHA256(checkValue);
        //    checkValue = checkValue.ToUpper();

        //    return checkValue;
        //}

        //private string GetSHA256(string value)
        //{
        //    var result = new StringBuilder();
        //    var sha256 = SHA256.Create();
        //    var bts = Encoding.UTF8.GetBytes(value);
        //    var hash = sha256.ComputeHash(bts);

        //    for (int i = 0; i < hash.Length; i++)
        //    {
        //        result.Append(hash[i].ToString("X2"));
        //    }

        //    return result.ToString();
        //}
    }
}
