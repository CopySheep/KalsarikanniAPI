using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services.Jill;
using HotelFuen31.APIs.Dto.Jill;
using HotelFuen31.APIs.Controllers.Yee;
using HotelFuen31.APIs.Dtos.Jill;

namespace HotelFuen31.APIs.Controllers.Jill
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallItemsController : ControllerBase
    {
        private readonly HallItemService _service;

        public HallItemsController(HallItemService service)
        {
            _service = service;
        }


        //GET: api/HallItems
       [HttpGet]
        public async Task<IEnumerable<HallItemDto>> GetHallItems()
        {
            var dtos = await _service.GetrAll().ToListAsync();

            var vms = dtos.Select(h => new HallItemDto
            {
                Id = h.Id,
                HallName = h.HallName,
                Capacity = h.Capacity,
                Description = h.Description,
                MinRent = h.MinRent,
                MaxRent = h.MaxRent,
                HallStatus = h.HallStatus,
                PhotoPath = h.PhotoPath,
                Ddescription = h.Ddescription,
                Location = h.Location,
            }).ToList();

            vms.ForEach(h =>
            {
                var pic = string.IsNullOrEmpty(h.PhotoPath) ? "noImage.png" : h.PhotoPath;
                h.PhotoPath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Content($"~/StaticFiles/Jill/{pic}")}";
            });


            return vms;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<HallItemDto>> GetHall(int id)
        {
            return await _service.GetHall(id).ToListAsync();
        }

    }
}
