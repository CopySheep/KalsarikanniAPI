using HotelFuen31.APIs.Dto.Jill;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using HotelFuen31.APIs.Dtos.Jill;

namespace HotelFuen31.APIs.Services.Jill
{
    public class HallItemService
    {
        private  AppDbContext _context;
        public HallItemService(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<HallItemDto> GetrAll()
        {
            var dto = _context.HallItems
                .AsNoTracking()
                .Select(h => new HallItemDto
                {
                    Id = h.Id,
                    HallName = h.HallName,
                    Capacity = h.Capacity,
                    Description = h.Description,
                    MinRent = h.MinRent,
                    MaxRent = h.MaxRent,
                    PhotoPath = h.PhotoPath,
                    HallStatus = h.HallStatus,
                    Ddescription= h.Ddescription,
                    Location = h.Location,

                });
            return dto;
        }

        public IQueryable<HallItemDto> GetHall(int id)
        {
            var query = _context.HallItems
                .AsNoTracking()
                .Where(h => h.Id == id)
                .Select(h => new HallItemDto
                {
                    Id = h.Id,
                    HallName = h.HallName,
                    Capacity = h.Capacity,
                    Description = h.Description,
                    MinRent = h.MinRent,
                    MaxRent = h.MaxRent,
                    PhotoPath = h.PhotoPath,
                    HallStatus = h.HallStatus,
                    Ddescription = h.Ddescription,
                    Location = h.Location,
                });

            return query;
        }
    }
}
