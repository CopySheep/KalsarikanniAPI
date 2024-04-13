using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Services.FC;
using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Interfaces.FC;
using HotelFuen31.APIs.Repository.FC;

namespace HotelFuen31.APIs.Controllers.FC
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationServiceDetailsController : ControllerBase
    {


        private readonly ReservationServService _service;

        public ReservationServiceDetailsController(ReservationServService service)
        {
            _service = service;
        }

		// GET: api/ReservationServiceDetails
		//      [HttpGet]
		//public async Task<IEnumerable<ReservationServiceDetailDto>> GetReservationServiceDetails()
		//{
		//	//return await _service.Read().ToListAsync();

		//          var dtos = await _service.Read().ToListAsync();
		//          var model = dtos.Select(x => new ReservationServiceDetailDto
		//          {
		//              Id = x.Id,
		//		ServicesTypeId = x.ServicesTypeId,
		//		ServiceDetailName = x.ServiceDetailName,
		//		Time = x.Time,
		//              Price = x.Price,
		//              Description = x.Description,
		//              ImgUrl = x.ImgUrl,

		//	}).ToList();

		//	model.ForEach(h =>
		//	{
		//		var pic = string.IsNullOrEmpty(h.ImgUrl) ? "noImage.png" : h.ImgUrl;
		//		h.ImgUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Content($"~/StaticFiles/FC/{pic}")}";
		//	});

		//	return model;
		//}


		// GET: api/ReservationServiceDetails/5
		//[HttpGet("{id}")]
		[HttpGet]
		public async Task<IEnumerable<ReservationServiceDetailDto>> GetReservationServiceDetail(int id)
		{
			var dtos = await _service.GetByid(id).ToListAsync();
			if (dtos == null) return new List<ReservationServiceDetailDto>();


			var model = dtos.Select(x => new ReservationServiceDetailDto
			{
				Id = x.Id,
				ServicesTypeId = x.ServicesTypeId,
				ServicesTypeName = x.ServicesTypeName,
				ServiceDetailName = x.ServiceDetailName,
				Time = x.Time,
				Price = x.Price,
				Description = x.Description,
				ImgUrl = x.ImgUrl,
			}).ToList();


			model.ForEach(h =>
			{
				var pic = string.IsNullOrEmpty(h.ImgUrl) ? "noImage.png" : h.ImgUrl;
				h.ImgUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Content($"~/StaticFiles/FC/{pic}")}";
			});

			return model;
		}




		//private IReservationServRepo GetRepo()
		//      {
		//          return new ReservationServEFRepo();
		//      }

		//      // GET: api/ReservationServiceDetails
		//      [HttpGet]
		//      public async Task<IEnumerable<ReservationServiceDetailDto>> GetReservationServiceDetails()
		//      {
		//          var service = new ReservationServService(GetRepo());
		//          return await service.Read().ToListAsync();
		//      }

		//// GET: api/ReservationServiceDetails/5
		//[HttpGet("{id}")]
		//public async Task<ActionResult<ReservationServiceDetail>> GetReservationServiceDetail(int id)
		//{
		//    var reservationServiceDetail = await _context.ReservationServiceDetails.FindAsync(id);

		//    if (reservationServiceDetail == null)
		//    {
		//        return NotFound();
		//    }

		//    return reservationServiceDetail;
		//}

		//// PUT: api/ReservationServiceDetails/5
		//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPut("{id}")]
		//public async Task<IActionResult> PutReservationServiceDetail(int id, ReservationServiceDetail reservationServiceDetail)
		//{
		//    if (id != reservationServiceDetail.Id)
		//    {
		//        return BadRequest();
		//    }

		//    _context.Entry(reservationServiceDetail).State = EntityState.Modified;

		//    try
		//    {
		//        await _context.SaveChangesAsync();
		//    }
		//    catch (DbUpdateConcurrencyException)
		//    {
		//        if (!ReservationServiceDetailExists(id))
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

		//// POST: api/ReservationServiceDetails
		//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPost]
		//public async Task<ActionResult<ReservationServiceDetail>> PostReservationServiceDetail(ReservationServiceDetail reservationServiceDetail)
		//{
		//    _context.ReservationServiceDetails.Add(reservationServiceDetail);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetReservationServiceDetail", new { id = reservationServiceDetail.Id }, reservationServiceDetail);
		//}

		//// DELETE: api/ReservationServiceDetails/5
		//[HttpDelete("{id}")]
		//public async Task<IActionResult> DeleteReservationServiceDetail(int id)
		//{
		//    var reservationServiceDetail = await _context.ReservationServiceDetails.FindAsync(id);
		//    if (reservationServiceDetail == null)
		//    {
		//        return NotFound();
		//    }

		//    _context.ReservationServiceDetails.Remove(reservationServiceDetail);
		//    await _context.SaveChangesAsync();

		//    return NoContent();
		//}

		//private bool ReservationServiceDetailExists(int id)
		//{
		//    return _context.ReservationServiceDetails.Any(e => e.Id == id);
		//}
	}
}
