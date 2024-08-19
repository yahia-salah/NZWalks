using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Application.Services
{
    public class DateConstantAppService : IDateConstantAppService
	{
		private readonly IptsdbContext dbContext;
		private readonly IMapper mapper;

		public DateConstantAppService(IptsdbContext dbContext,IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
		}

		public async Task<DateConstantDto> CreateAsync(CreateDateConstantDto createDateConstantDto)
		{
			var newRecord = mapper.Map<DateConstant>(createDateConstantDto);
			newRecord.IsSystemDate = createDateConstantDto.IsSystemDate ?? false; // Can we default this to false? Or user can set this?
			await dbContext.Set<DateConstant>().AddAsync(newRecord);
			await dbContext.SaveChangesAsync();

			return mapper.Map<DateConstantDto>(newRecord);
		}

		public async Task<DateConstantDto> DeleteAsync(Guid id)
		{
			var existingRecord = await dbContext.Set<DateConstant>().AsQueryable().FirstOrDefaultAsync(dc => dc.Id == id);

			if (existingRecord == null)
			{
				return null;
			}
			dbContext.Set<DateConstant>().Remove(existingRecord);
			await dbContext.SaveChangesAsync();
			return mapper.Map<DateConstantDto>(existingRecord);
		}

		public Task<IQueryable<DateConstantDto>> GetAllAsync()
		{
			// If there is delete functionality, then the below code should be modified to exclude the deleted records (maybe add deleted column in table?)
			return Task.FromResult(dbContext.Set<DateConstant>().OrderBy(dc=>dc.Value).Select(dc => mapper.Map<DateConstantDto>(dc)));
		}

		public async Task<DateConstantDto> GetByIdAsync(Guid id)
		{
			var existingRecord = await dbContext.Set<DateConstant>().AsQueryable().FirstOrDefaultAsync(dc => dc.Id == id);

			if (existingRecord == null)
			{
				return null;
			}

			return mapper.Map<DateConstantDto>(existingRecord);
		}

		public async Task<DateConstantDto> UpdateAsync(Guid id, UpdateDateConstantDto updateDateConstantDto)
		{
			var existingRecord = await dbContext.Set<DateConstant>().AsQueryable().FirstOrDefaultAsync(dc => dc.Id == id);

			if (existingRecord == null)
			{
				return null;
			}

			mapper.Map(updateDateConstantDto, existingRecord);
			await dbContext.SaveChangesAsync();

			return mapper.Map<DateConstantDto>(existingRecord);
		}
	}
}
