using NZWalks.API.Models.DTO;

namespace NZWalks.API.Application.Services
{
	public interface IDateConstantAppService
	{
		Task<DateConstantDto> CreateAsync(CreateDateConstantDto createDateConstantDto);
		Task<DateConstantDto> UpdateAsync(Guid id, UpdateDateConstantDto updateDateConstantDto);
		Task<IQueryable<DateConstantDto>> GetAllAsync();
		Task<DateConstantDto> GetByIdAsync(Guid id);
		Task<DateConstantDto> DeleteAsync(Guid id);
	}
}
