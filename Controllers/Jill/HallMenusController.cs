using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Dto.Jill;
using HotelFuen31.APIs.Services.Jill;
using HotelFuen31.APIs.Dtos.Jill;

namespace HotelFuen31.APIs.Controllers.Jill
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallMenusController : ControllerBase
    {
        private readonly HallMenuService _service;

        public HallMenusController(HallMenuService service)
        {
            _service = service;
        }

        // GET: api/HallItems
        [HttpGet("HallMenus")]
        public async Task<IEnumerable<HallMenuDto>> GetHallMenus()
        {
            return await _service.GetAll().ToListAsync();
        }

        // GET: api/HallItems/Category
        [HttpGet("Category")]
        public async Task<IEnumerable<HallCategoryDto>> GetCategory()
        {
            return await _service.Getrcategory().ToListAsync();
        }

        //GET: api/HallItems/Category/{Categoryid}
        [HttpGet("SearchMenu")]
        public async Task<IEnumerable<HallMenuDto>> SearchMenu([FromQuery] HallSearchDto search)
        {
            return await _service.SearchMenu(search).ToListAsync();
        }

        // GET: api/HallItems/Menu/{Menuid}
        [HttpGet("Menu/{Menuid}")]
        public async Task<IEnumerable<HallMenuDto>> GetMenu(int Menuid)
        {
            return await _service.GetMenu(Menuid).ToListAsync();
        }

    }
}
