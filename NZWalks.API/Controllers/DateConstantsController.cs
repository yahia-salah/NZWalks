using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Application.Services;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DateConstantsController : ControllerBase
	{
		private readonly IDateConstantAppService dateConstantApp;

		public DateConstantsController(IDateConstantAppService dateConstantApp)
		{
			this.dateConstantApp = dateConstantApp;
		}

		// GET: api/DateConstants
		[HttpGet]
		public async Task<IActionResult> GetDateConstants()
		{
			var dateConstants = dateConstantApp.GetAllAsync();
			return Ok(dateConstants);
		}

		// GET: api/DateConstants/5
		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetDateConstant(Guid id)
		{
			var dateConstant = await dateConstantApp.GetByIdAsync(id);

			if(dateConstant == null)
			{
				return NotFound();
			}

			return Ok(dateConstant);
		}

		// POST: api/DateConstants
		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> PostDateConstant([FromBody] CreateDateConstantDto createDateConstantDto)
		{
			var newDateConstant = await dateConstantApp.CreateAsync(createDateConstantDto);
			return CreatedAtAction(nameof(GetDateConstant), new { id = newDateConstant.Id }, newDateConstant);
		}

		// PUT: api/DateConstants/5
		[HttpPut("{id:Guid}")]
		[ValidateModel]
		public async Task<IActionResult> PutDateConstant(Guid id, [FromBody] UpdateDateConstantDto updateDateConstantDto)
		{
			var updatedDateConstant = await dateConstantApp.UpdateAsync(id, updateDateConstantDto);

			if(updatedDateConstant == null)
			{
				return NotFound();
			}

			return Accepted(updatedDateConstant);
		}

		// DELETE: api/DateConstants/5
		[HttpDelete("{id:Guid}")]
		public async Task<IActionResult> DeleteDateConstant(Guid id)
		{
			var deletedDateConstant = await dateConstantApp.DeleteAsync(id);

			if(deletedDateConstant == null)
			{
				return NotFound();
			}

			return Ok(deletedDateConstant);
		}
	}
}
