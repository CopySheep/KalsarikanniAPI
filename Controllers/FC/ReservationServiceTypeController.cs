using HotelFuen31.APIs.Dtos.FC;
using HotelFuen31.APIs.Services.FC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelFuen31.APIs.Controllers.FC
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationServiceTypeController : ControllerBase
	{

		private readonly ReservationServTypeService _service;
		public ReservationServiceTypeController(ReservationServTypeService service)
		{
			_service = service;
		}

		// GET: api/ReservationServiceType
		[HttpGet]
		public async Task<IEnumerable<ReservationServiceTypeDto>> GetReservationServiceDetails()
		{
			//return await _service.Read().ToListAsync();

			var dtos = await _service.Read().ToListAsync();
			var model = dtos.Select(x => new ReservationServiceTypeDto
			{
				Id = x.Id,
				ServicesTypeName = x.ServicesTypeName,
				Description = x.Description,
				ImgUrl = x.ImgUrl
			}).ToList();

			model.ForEach(h =>
			{
				var pic = string.IsNullOrEmpty(h.ImgUrl) ? "noImage.png" : h.ImgUrl;
				h.ImgUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Content($"~/StaticFiles/FC/{pic}")}";
			});


			return model;


		}
	}
}
