using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
	// https://localhost:5001/api/regions
	[Route("api/[controller]")]
	[ApiController]
	public class RegionsController : ControllerBase
	{
		private readonly NZWalksDbContext _dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext=dbContext;
        }

		// GET: api/regions
		[HttpGet]
		public IActionResult GetAll()
		{
			var regions = _dbContext.Regions.ToList();

			var regionsDto = regions.Select(r => new RegionDTO()
			{
				Id = r.Id,
				Code = r.Code,
				Name = r.Name,
				ImageUrl = r.ImageUrl
			});
			
			return Ok(regionsDto);
		}

		// GET: api/regions/{id}
		[HttpGet("{id}")]
		public IActionResult Get(Guid id)
		{
			var region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

			if (region == null)
			{
				return NotFound();
			}

			var regionDto = new RegionDTO()
			{
				Id = region.Id,
				Code = region.Code,
				Name = region.Name,
				ImageUrl = region.ImageUrl
			};

			return Ok(regionDto);
		}

		// POST: api/regions
		[HttpPost]
		public IActionResult AddRegion([FromBody] AddRegionRequestDTO regionDto)
		{
			var regionDomainModel = new Region()
			{
				Code = regionDto.Code,
				Name = regionDto.Name,
				ImageUrl = regionDto.ImageUrl
			};

			_dbContext.Regions.Add(regionDomainModel);
			_dbContext.SaveChanges();

			var regionDtoResponse= new RegionDTO()
			{
				Id = regionDomainModel.Id,
				Code = regionDomainModel.Code,
				Name = regionDomainModel.Name,
				ImageUrl = regionDomainModel.ImageUrl
			};

			return CreatedAtAction(nameof(Get),new {id=regionDomainModel.Id}, regionDtoResponse);
		}

		// PUT: api/regions/{id}
		[HttpPut]
		[Route("{id}")]
		public IActionResult UpdateRegion([FromRoute]Guid id, [FromBody] UpdateRegionRequestDTO regionDto)
		{
			var region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

			if (region == null)
			{
				return NotFound();
			}

			region.Code = regionDto.Code;
			region.Name = regionDto.Name;
			region.ImageUrl = regionDto.ImageUrl;

			_dbContext.SaveChanges();

			var regionDtoResponse = new RegionDTO()
			{
				Id = region.Id,
				Code = region.Code,
				Name = region.Name,
				ImageUrl = region.ImageUrl
			};

			return Ok(regionDtoResponse);
		}

		// DELETE: api/regions/{id}
		[HttpDelete]
		[Route("{id}")]
		public IActionResult DeleteRegion([FromRoute]Guid id)
		{
			var region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

			if (region == null)
			{
				return NotFound();
			}

			_dbContext.Regions.Remove(region);
			_dbContext.SaveChanges();

			return NoContent();
		}
	}
}
