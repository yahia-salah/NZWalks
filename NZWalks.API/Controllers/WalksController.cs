using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	// /api/walks
	[Route("api/[controller]")]
	[ApiController]
	public class WalksController : ControllerBase
	{
		private readonly IWalkRepository _walkRepository;
		private readonly IMapper _mapper;

		public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
			_walkRepository = walkRepository;
			_mapper = mapper;
		}

        // Create Walk
        // POST /api/walks
        [HttpPost]
		[ValidateModel]
		public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
		{
			var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);
			var walk =  await _walkRepository.CreateWalkAsync(walkDomainModel);
			var walkDTOResponse = _mapper.Map<WalkDTO>(walk);
			return CreatedAtAction(nameof(Get),new { id=walk.Id },walkDTOResponse);
		}

		// Get Walk by Id
		// GET /api/walks/{id}
		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var walk = await _walkRepository.GetWalkByIdAsync(id);
			if (walk == null)
			{
				return NotFound();
			}
			var walkDTO = _mapper.Map<WalkDTO>(walk);
			return Ok(walkDTO);
		}

		// Get all Walks
		// GET /api/walks?filterOn={filterOn}&filterQuery={filterQuery}
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery]string? filterOn, [FromQuery]string? filterQuery, [FromQuery]string? sortBy, [FromQuery]bool? isAscending)
		{
			var walks = await _walkRepository.GetWalksAsync(filterOn,filterQuery,sortBy,isAscending);
			var walksDTO = _mapper.Map<List<WalkDTO>>(walks);
			return Ok(walksDTO);
		}	

		// Get Walks by Region Id
		// GET /api/walks/region/{regionId}
		[HttpGet("region/{regionId:Guid}")]
		public async Task<IActionResult> GetByRegionId(Guid regionId)
		{
			var walks = await _walkRepository.GetWalksByRegionIdAsync(regionId);
			var walksDTO = _mapper.Map<List<WalkDTO>>(walks);
			return Ok(walksDTO);
		}	

		// Get Walks by Difficulty Id
		// GET /api/walks/difficulty/{difficultyId}
		[HttpGet("difficulty/{difficultyId:Guid}")]
		public async Task<IActionResult> GetByDifficultyId(Guid difficultyId)
		{
			var walks = await _walkRepository.GetWalksByDifficultyIdAsync(difficultyId);
			var walksDTO = _mapper.Map<List<WalkDTO>>(walks);
			return Ok(walksDTO);
		}
		
		// Update Walk
		// PUT /api/walks/{id}
		[HttpPut("{id:Guid}")]
		[ValidateModel]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
		{
			var walk = await _walkRepository.UpdateWalkAsync(id,_mapper.Map<Walk>(updateWalkRequestDTO));
			if (walk == null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<WalkDTO>(walk));
		}	

		// Delete Walk
		// DELETE /api/walks/{id}
		[HttpDelete("{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var walk = await _walkRepository.DeleteWalkAsync(id);
			if (walk == null)
			{
				return NotFound();
			}
			return NoContent();
		}	
	}
}
