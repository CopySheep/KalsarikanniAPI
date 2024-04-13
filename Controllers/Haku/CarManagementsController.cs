using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelFuen31.APIs.Models;

namespace HotelFuen31.APIs.Controllers.Haku
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarManagementsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public CarManagementsController(AppDbContext context)
		{
			_context = context;
		}



		// 用來取得所有車輛資料，並回傳給前端
		// GET: api/CarManagements
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CarManagement>>> GetCarManagements()
		{
			var remainedCars= _context.CarManagements.Where(x => x.Status == true).ToList();
			return await _context.CarManagements.ToListAsync();
		}

		// 用來取得特定車輛資料，並回傳給前端，這邊的 id 是車輛的編號
		// GET: api/CarManagements/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CarManagement>> GetCarManagement(int id)
		{
			var carManagement = await _context.CarManagements.FindAsync(id);

			if (carManagement == null)
			{
				return NotFound();
			}

			return carManagement;
		}

		// PUT 是用來更新車輛資料，這邊的 id 是車輛的編號，並且會將更新後的資料回傳給前端
		// PUT: api/CarManagements/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCarManagement(int id, CarManagement carManagement)
		{
			// carManagement 是更新後的車輛資料，根據車輛的編號找到該車輛的資料，並將該車輛的資料更新，Entity Framework Core 會自動追蹤資料的狀態，並且告訴 Entity Framework Core 要更新哪些資料

			// 如果更新的車輛的編號與 id 不同，則回傳 BadRequest
			if (id != carManagement.Id)
			{
				return BadRequest();
			}

			// 將更新後的資料設定為已修改， Entry 方法是用來追蹤資料的狀態，並且告訴 Entity Framework Core 要更新哪些資料，State 是用來設定資料的狀態，	 EntityState 是用來設定資料的狀態，Modified 是用來設定資料的狀態為已修改
			_context.Entry(carManagement).State = EntityState.Modified;

			try
			{
				// 儲存更新後的資料
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) // 如果更新失敗，則回傳 NotFound
			{
				if (!CarManagementExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST 是用來新增車輛資料，並且會將新增後的資料回傳給前端
		// POST: api/CarManagements
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<CarManagement>> PostCarManagement(CarManagement carManagement)
		{
			_context.CarManagements.Add(carManagement);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCarManagement", new { id = carManagement.Id }, carManagement);
		}

		// DELETE 是用來刪除車輛資料，這邊的 id 是車輛的編號
		// DELETE: api/CarManagements/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCarManagement(int id)
		{
			var carManagement = await _context.CarManagements.FindAsync(id);
			if (carManagement == null)
			{
				return NotFound();
			}

			_context.CarManagements.Remove(carManagement);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CarManagementExists(int id)
		{
			return _context.CarManagements.Any(e => e.Id == id);
		}
	}
}
