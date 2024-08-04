using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories
{
	public interface IWalkRepository
	{
		Task<Walk> CreateWalkAsync(Walk walk);
		Task<Walk> GetWalkByIdAsync(Guid id);
		Task<IEnumerable<Walk>> GetWalksAsync(string? filterOn=null,string? filterQuery=null,string? sortBy=null,bool? isAscending=true);
		Task<IEnumerable<Walk>> GetWalksByRegionIdAsync(Guid regionId);
		Task<IEnumerable<Walk>> GetWalksByDifficultyIdAsync(Guid difficultyId);
		Task<Walk> UpdateWalkAsync(Guid id, Walk walk);
		Task<Walk> DeleteWalkAsync(Guid id);
	}
}
