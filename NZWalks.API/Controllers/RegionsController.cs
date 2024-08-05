using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	// https://localhost:5001/api/regions
	[Route("api/[controller]")]
	[ApiController]
	public class RegionsController : ControllerBase
	{
		private readonly IRegionRepository _regionRepository;
		private readonly IMapper _mapper;

		public RegionsController(IRegionRepository regionRepository,IMapper mapper)
		{
			_regionRepository = regionRepository;
			_mapper = mapper;
		}

		// GET: api/regions
		[HttpGet]
		[Authorize(Roles ="Reader,Writer")]
		public async Task<IActionResult> GetAll()
		{
			var regions = await _regionRepository.GetAllAsync();

			var regionsDto = _mapper.Map<List<RegionDTO>>(regions);

			return Ok(regionsDto);
		}

		// GET: api/regions/{id}
		[HttpGet("{id}")]
		[Authorize(Roles ="Reader,Writer")]
		public async Task<IActionResult> Get(Guid id)
		{
			var region = await _regionRepository.GetByIdAsync(id);

			if (region == null)
			{
				return NotFound();
			}

			var regionDto = _mapper.Map<RegionDTO>(region);

			return Ok(regionDto);
		}

		// POST: api/regions
		[HttpPost]
		[ValidateModel]
		[Authorize(Roles ="Writer")]
		public async Task<IActionResult> AddRegion([FromBody] AddRegionRequestDTO regionDto)
		{	
			var regionDomainModel = await _regionRepository.CreateAsync(_mapper.Map<Region>(regionDto));

			var regionDtoResponse = _mapper.Map<RegionDTO>(regionDomainModel);

			return CreatedAtAction(nameof(Get), new { id = regionDomainModel.Id }, regionDtoResponse);
		}

		// PUT: api/regions/{id}
		[HttpPut]
		[Route("{id}")]
		[ValidateModel]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO regionDto)
		{
			var region = await _regionRepository.UpdateAsync(id, _mapper.Map<Region>(regionDto));

			if (region == null)
			{
				return NotFound();
			}

			var regionDtoResponse = _mapper.Map<RegionDTO>(region);

			return Ok(regionDtoResponse);
		}

		// DELETE: api/regions/{id}
		[HttpDelete]
		[Route("{id}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
		{
			var region = await _regionRepository.DeleteAsync(id);

			if (region == null)
			{
				return NotFound();
			}
			
			return NoContent();
		}
	}
}
