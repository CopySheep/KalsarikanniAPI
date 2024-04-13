using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services;
using HotelFuen31.APIs.Dtos.Chen;


namespace HotelFuen31.APIs.Controllers.Chen
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly RoomTypeService _service;
        public RoomTypesController(RoomTypeService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("GetRoomTypes")]
        public async Task<ActionResult<IEnumerable<RoomTypeDtos>>> GetRoomTypes(int id)
        {

            return await _service.GetAllRoomTypes(id).ToListAsync();
        }

        
        [HttpGet]
        [Route("GetRoomDetail")]
        public async Task<ActionResult<IEnumerable<RoomDetailDtos>>> GetRoomDetail(int id)
        {
            return await _service.GetRoomDetail(id).ToListAsync();
        }

        [HttpGet]
        [Route("GetCheckRoomDetail")]
        public async Task<ActionResult<IEnumerable<CheckRoomDto>>> GetCheckRoomDetail(string b_date,string e_date)
        {
            return  _service.GetCheckRoomData(b_date, e_date);
        }

        //// GET: api/RoomTypes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<RoomType>> GetRoomType(int id)
        //{
        //    var roomType = await _context.RoomTypes.FindAsync(id);

        //    if (roomType == null)
        //    {
        //        return NotFound();
        //    }

        //    return roomType;
        //}

        //// PUT: api/RoomTypes/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRoomType(int id, RoomType roomType)
        //{
        //    if (id != roomType.RoomTypeId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(roomType).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RoomTypeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/RoomTypes
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<RoomType>> PostRoomType(RoomType roomType)
        //{
        //    _context.RoomTypes.Add(roomType);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRoomType", new { id = roomType.RoomTypeId }, roomType);
        //}

        //// DELETE: api/RoomTypes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRoomType(int id)
        //{
        //    var roomType = await _context.RoomTypes.FindAsync(id);
        //    if (roomType == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.RoomTypes.Remove(roomType);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool RoomTypeExists(int id)
        //{
        //    return _context.RoomTypes.Any(e => e.RoomTypeId == id);
        //}
    }
}
