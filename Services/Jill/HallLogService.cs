using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Dtos.Jill;
using HotelFuen31.APIs.Dtos.RenYu;
using HotelFuen31.APIs.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Services.Jill
{
    public class HallLogService
    {
        private readonly AppDbContext _context;
        public HallLogService(AppDbContext context)
        { 
            _context = context; 
        }

        public async Task<string> Create(CreateHallLogDto dto)
        {
            var HallLog = new HallLog
            {
                Name = dto.Name,
                HallId = dto.HallId,
                CellPhone = dto.CellPhone,
                Email = dto.Email,
                Guests = dto.Guests,
                BookingStatus = true,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
            };
            _context.HallLogs.Add(HallLog);
            await _context.SaveChangesAsync();

            for (int i = 0; i < dto.HallMenuId.Count; i++)
            {
                var HallOrderItem = new HallOrderItem
                {
                    HallLogId = HallLog.Id,
                    Price = dto.Price[i],
                    Qty = dto.Qty[i],
                };
            

            _context.HallOrderItems.Add(HallOrderItem);
            await _context.SaveChangesAsync();

            var HallMenuSchedule = new HallMenuSchedule
            {
                HallMenuId = dto.HallMenuId[i],
                HallOrderItemId = HallOrderItem.Id,
            };

            _context.HallMenuSchedules.Add(HallMenuSchedule);
            await _context.SaveChangesAsync();

            };

            return "新增訂單成功";
        }



        public IQueryable<HallLogDto> GetAll() {

            var dto = _context.HallLogs
                .AsNoTracking()
                .Select(h => new HallLogDto
                {
                    Id = h.Id,
                    HallId = h.HallId,
                    UserId = h.UserId,
                    Guests = h.Guests,
                    StartTime = h.StartTime,
                    EndTime = h.EndTime,
                    BookingStatus = h.BookingStatus,
                    Name = h.Name,
                    CellPhone = h.CellPhone,
                    Email = h.Email,
                    FilePath = h.FilePath,
                    HallName = h.Hall.HallName,
                });

            return dto;
        }

        public IQueryable<HallLogDto> SearchLog(int id) 
        {
            var query = _context.HallLogs
                .AsNoTracking()
                .Where(h => h.Hall.Id == id)
                .Select(h => new HallLogDto
                {
                    Id = h.Id,
                    HallId = h.HallId,
                    UserId = h.UserId,
                    Guests = h.Guests,
                    StartTime = h.StartTime,
                    EndTime = h.EndTime,
                    BookingStatus = h.BookingStatus,
                    Name = h.Name,
                    CellPhone = h.CellPhone,
                    Email = h.Email,
                    FilePath = h.FilePath,
                    HallName = h.Hall.HallName,
                });

            return query;
        }


    }
}
