using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
	public class SQLRegionRepository : IRegionRepository
	{
		private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

		public async Task<Region> CreateAsync(Region region)
		{
			var regionDomainModel = new Region()
			{
				Code = region.Code,
				Name = region.Name,
				ImageUrl = region.ImageUrl
			};

			await _dbContext.Regions.AddAsync(regionDomainModel);
			await _dbContext.SaveChangesAsync();

			return regionDomainModel;
		}

		public async Task<Region?> DeleteAsync(Guid id)
		{
			var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
			if (existingRegion == null)
			{
				return null;
			}
			_dbContext.Regions.Remove(existingRegion);
			await _dbContext.SaveChangesAsync();
			return existingRegion;
		}

		public async Task<List<Region>> GetAllAsync()
		{
			var regions = await _dbContext.Regions.ToListAsync();
			return regions;
		}

		public async Task<Region> GetByIdAsync(Guid id)
		{
			var region = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
			return region;
		}

		public async Task<Region?> UpdateAsync(Guid id, Region region)
		{
			var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
			if (existingRegion == null)
			{
				return null;
			}
			existingRegion.Code = region.Code;
			existingRegion.Name = region.Name;
			existingRegion.ImageUrl = region.ImageUrl;

			await _dbContext.SaveChangesAsync();
			return existingRegion;
		}
	}
}
