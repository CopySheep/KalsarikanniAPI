using HotelFuen31.APIs.Dtos.Chen;
using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services.Yee;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Services
{
    public class RoomTypeService
    {
        private readonly AppDbContext _context;

        public RoomTypeService(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<RoomTypeDtos> GetAllRoomTypes(int id)
        {

            var query = _context.RoomTypes
                .Where(x => (x.RoomTypeId == id || id==0))
                .Select(rr => new RoomTypeDtos
                {
                    RoomTypeId = rr.RoomTypeId,
                    TypeName = rr.TypeName,
                    Description = rr.Description,
                    Capacity = rr.Capacity,
                    BedType = rr.BedType,
                    RoomCount = rr.RoomCount,
                    ImageUrl = rr.ImageUrl,
                    WeekdayPrice = rr.WeekdayPrice,
                    Size = rr.Size,
                    ImgList = _context.RoomDetailImgs
                             .Where(img => img.RoomTypeId == rr.RoomTypeId)
                              .Select(img => new RoomDetailImgDtos
                                 {
                                     RoomTypeId = img.RoomTypeId,
                                       ImgSeq = img.ImgSeq,
                              ImgUrl = img.ImgUrl
                     })
    .ToList()


        });
            return query;
        }
  public IQueryable<RoomDetailDtos> GetRoomDetail(int id)
{
    var query = _context.RoomDaysPrices
        .Where(rr => rr.RoomTypeId == id)
        .Select(rr => new RoomDetailDtos
        {
            RoomTypeId = rr.RoomTypeId,
            Date = rr.Date,
            Description = rr.Description,
            IsHoliday = rr.IsHoliday,
            Price = rr.Price,

        });

    return query;
}



        public List<CheckRoomDto> GetCheckRoomData(string start, string end)
        {
            var data = new CartRoomService(_context).GetRoomStock(start, end);
            List<CheckRoomDto> list = new List<CheckRoomDto>();
            if (data.RoomStocks != null)
            {
                foreach (var item in data.RoomStocks)
                {
                    int totalPrice = _context.RoomDaysPrices
                                        .Where(x => x.Date >= Convert.ToDateTime(data.CheckInDate) &&
                                                    x.Date < Convert.ToDateTime(data.CheckOutDate) &&
                                                    x.RoomTypeId == item.Id)
                                        .Sum(x => x.Price);
                    var checkRoomDto = new CheckRoomDto
                    {
                        RoomTypeId = item.Id,
                        TypeName = item.Name,
                        Capacity = item.Capacity,
                        BedType = item.BedType,
                        Description = item.Desc,
                        Size = item.Size,
                        SumPrice = totalPrice,
                        CanSoldQty = item.Rooms.Count(),
                        ImageUrl = item.Picture

                    };
                    list.Add(checkRoomDto);
                }
            }

            return list;
        }

    }
}
