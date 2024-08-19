using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Application.Services;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HoldDatesController : ControllerBase
	{
		private readonly IHoldDateAppService holdDateAppService;

		public HoldDatesController(IHoldDateAppService holdDateAppService)
		{
			this.holdDateAppService = holdDateAppService;
		}

		// GET: api/HoldDates
		[HttpGet]
		public async Task<IActionResult> GetHoldDates()
		{
			var holdDates = holdDateAppService.GetAllAsync();
			return Ok(holdDates);
		}

		// PUT: api/HoldDates
		[HttpPut]
		public async Task<IActionResult> PutHoldDates([FromBody] HoldDatesUpdateDto holdDatesUpdateDto)
		{
			var validationResults = await holdDateAppService.ValidateBeforeUpdateAsync(holdDatesUpdateDto);

			if (validationResults.Any(x => !x.IsValid))
			{
				return BadRequest(validationResults);
			}

			var updatedHoldDates = await holdDateAppService.UpdateAllAsync(holdDatesUpdateDto);
			return Accepted(updatedHoldDates);
		}
	}
}
