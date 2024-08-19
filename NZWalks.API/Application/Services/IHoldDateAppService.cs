using NZWalks.API.Models.DTO;

namespace NZWalks.API.Application.Services
{
	public interface IHoldDateAppService
	{
		Task<List<HoldDateDto>> UpdateAllAsync(HoldDatesUpdateDto holdDatesUpdateDto);
		Task<IQueryable<HoldDateDto>> GetAllAsync();
		Task<List<HoldDateValidationDto>> ValidateBeforeUpdateAsync(HoldDatesUpdateDto holdDatesUpdateDto);
	}
}
