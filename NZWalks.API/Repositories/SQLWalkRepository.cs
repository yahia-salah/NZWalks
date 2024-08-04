using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories
{
	public class SQLWalkRepository : IWalkRepository
	{
		private readonly NZWalksDbContext _dbContext;

		public SQLWalkRepository(NZWalksDbContext dbContext)
        {
			_dbContext = dbContext;
		}

        public async Task<Walk> CreateWalkAsync(Walk walk)
		{
			var walkDomainModel = new Walk()
			{
				Name = walk.Name,
				Description = walk.Description,
				ImageUrl = walk.ImageUrl,
				LengthInKm = walk.LengthInKm,
				RegionId = walk.RegionId,
				DifficultyId = walk.DifficultyId
			};	

			await _dbContext.Walks.AddAsync(walkDomainModel);
			await _dbContext.SaveChangesAsync();
			return walkDomainModel;
		}

		public async Task<Walk> DeleteWalkAsync(Guid id)
		{
			var walk = await _dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(w => w.Id == id);

			if (walk == null)
			{
				return null;
			}
			_dbContext.Walks.Remove(walk);
			await _dbContext.SaveChangesAsync();
			return walk;
		}

		public async Task<Walk> GetWalkByIdAsync(Guid id)
		{
			var walk = await _dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(w => w.Id == id);
			return walk;
		}

		public async Task<IEnumerable<Walk>> GetWalksAsync(string? filterOn=null,string? filterQuery=null, string? sortBy = null, bool? isAscending = true)
		{
			var walks = _dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

			// filtering
			if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
			{
				switch (filterOn.ToLower())
				{
					case "name":
						walks = walks.Where(w => w.Name.ToLower().Contains(filterQuery.ToLower()));
						break;
					case "description":
						walks = walks.Where(w => w.Description.ToLower().Contains(filterQuery.ToLower()));
						break;
				}
			}

			// sorting
			if (!string.IsNullOrEmpty(sortBy))
			{
				switch (sortBy.ToLower())
				{
					case "name":
						walks = isAscending == true ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
						break;
					case "description":
						walks = isAscending == true ? walks.OrderBy(w => w.Description) : walks.OrderByDescending(w => w.Description);
						break;
					case "lengthinkm":
						walks = isAscending == true ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
						break;
				}
			}

			return await walks.ToListAsync();
		}

		public async Task<IEnumerable<Walk>> GetWalksByDifficultyIdAsync(Guid difficultyId)
		{
			return await _dbContext.Walks.Include("Region").Include("Difficulty").Where(w => w.DifficultyId == difficultyId).ToListAsync();
		}

		public async Task<IEnumerable<Walk>> GetWalksByRegionIdAsync(Guid regionId)
		{
			return await _dbContext.Walks.Include("Region").Include("Difficulty").Where(w => w.RegionId == regionId).ToListAsync();
		}

		public async Task<Walk> UpdateWalkAsync(Guid id,Walk walk)
		{
			var existingWalk = await _dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(w => w.Id == id);
			if (existingWalk == null)
				return null;

			existingWalk.Name = walk.Name;
			existingWalk.Description = walk.Description;
			existingWalk.ImageUrl = walk.ImageUrl;
			existingWalk.LengthInKm = walk.LengthInKm;
			existingWalk.RegionId = walk.RegionId;
			existingWalk.DifficultyId = walk.DifficultyId;

			await _dbContext.SaveChangesAsync();
			return existingWalk;
		}
	}
}
