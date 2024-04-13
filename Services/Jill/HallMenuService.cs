using HotelFuen31.APIs.Dto.Jill;
using HotelFuen31.APIs.Dtos.Jill;
using HotelFuen31.APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Services.Jill
{
    public class HallMenuService
    {

        private readonly AppDbContext _context;
        public HallMenuService(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<HallMenuDto> GetAll()
        {
            var dto = _context.HallMenus
                .AsNoTracking()
                .Select(h => new HallMenuDto
                {
                    Id = h.Id,
                    DishName = h.DishName,
                    Description = h.Description,
                    Price = h.Price,
                    CategoryId = h.CategoryId,
                    CategoryName = h.Category.Category,
                    Keywords = h.Keywords,
                });

            return dto;
        }

        public IQueryable<HallCategoryDto> Getrcategory()
        {
            var dto = _context.HallDishCategories
                .AsNoTracking()
                .Select(h => new HallCategoryDto
                {
                    Id = h.Id,
                    Category = h.Category,
                });

            return dto;
        }

        public IQueryable<HallMenuDto> SearchMenu(HallSearchDto search)
        {
            //如果CategoryId =0 或是 關鍵字=null 就顯示全部
            var query = (search.CategoryId == 0 || search.CategoryId == null)  ? 
                _context.HallMenus.AsNoTracking() 
                : _context.HallMenus.AsNoTracking().Where(h => h.CategoryId == search.CategoryId);

            if (!string.IsNullOrEmpty(search.Keywords))
            {
                query = query.Where(h => h.Keywords.Contains(search.Keywords));
            }

            return query.Select(h => new HallMenuDto
            {
                Id = h.Id,
                DishName = h.DishName,
                Description = h.Description,
                Price = h.Price,
                CategoryId = h.CategoryId,
                CategoryName = h.Category.Category,
                Keywords = h.Keywords
            });
        }


        public IQueryable<HallMenuDto> GetMenu(int id)
        {
            var query = _context.HallMenus
                .AsNoTracking()
                .Where(h => h.Id == id)
                .Select(h => new HallMenuDto
                {
                    Id = h.Id,
                    DishName = h.DishName,
                    Description = h.Description,
                    Price = h.Price,
                    CategoryId = h.CategoryId,
                    CategoryName = h.Category.Category,
                    Keywords = h.Keywords,
                });

            return query;
        }

    }
}
