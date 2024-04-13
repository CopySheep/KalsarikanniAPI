using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Dtos.Jill
{
    public class HallMenuDto
    {
        public int Id { get; set; }

        public string DishName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string PhotoPath { get; set; }

        public string CategoryName { get; set; }

        public string Keywords { get; set; }

        public virtual HallDishCategory Category { get; set; }
    }
}
