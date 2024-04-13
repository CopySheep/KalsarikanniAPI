using HotelFuen31.APIs.Dtos.Yee;
using HotelFuen31.APIs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Controllers.Yee
{
    [Route("api/[controller]")]
    [ApiController]
    public class YeeController : ControllerBase
    {
        private readonly AppDbContext _db;
        public YeeController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Yee/
        
    }
}
