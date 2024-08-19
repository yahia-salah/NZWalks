using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Application.Services
{
	public class HoldDateAppService : IHoldDateAppService
	{
		private readonly IptsdbContext dbContext;
		private readonly IMapper mapper;
		private readonly Dictionary<string, int> holdDateRules = new Dictionary<string, int>
		{
			{ "May - June", 5 },
			{ "June - July", 6 },
			{ "July - Sep", 6 }
		};
		private readonly Dictionary<string, int> releaseDateRules = new Dictionary<string, int>
		{
			{ "May - June", 7 },
			{ "June - July", 7 },
			{ "July - Sep", 9 }
		};

		public HoldDateAppService(IptsdbContext dbContext, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
		}

		public Task<IQueryable<HoldDateDto>> GetAllAsync()
		{
			return Task.FromResult(dbContext.Set<HoldDate>().Select(dc => mapper.Map<HoldDateDto>(dc)));
		}

		public async Task<List<HoldDateValidationDto>> ValidateBeforeUpdateAsync(HoldDatesUpdateDto holdDatesUpdateDto)
		{
			var existingRecords = await dbContext.Set<HoldDate>().ToListAsync();
			var validationResults = new List<HoldDateValidationDto>();

			foreach(var holdDateUpdateDto in holdDatesUpdateDto.Updates)
			{
				var record = existingRecords.FirstOrDefault(hd => hd.Name == holdDateUpdateDto.Name);

				if (record == null)
				{
					validationResults.Add(new HoldDateValidationDto
					{
						IsValid = false,
						Message = $"'{holdDateUpdateDto.Name}' is invalid Name"
					});
					continue;
				}

				// Needs confirmation with PM
				if(holdDateUpdateDto.BeginHold == null && holdDateUpdateDto.Release == null)
					{
					validationResults.Add(new HoldDateValidationDto
					{
						IsValid = false,
						Message = $"Begin hold and Release cannot be empty for '{holdDateUpdateDto.Name}'"
					});
				}

				if (holdDateUpdateDto.BeginHold != null && holdDateUpdateDto.BeginHold.Value.Month != holdDateRules[record.Name])
				{
					validationResults.Add(new HoldDateValidationDto
					{
						IsValid = false,
						Message = $"Begin hold date of '{record.Name}' is not in {GetMonthName(holdDateRules[record.Name])}"
					});
				}

				if (holdDateUpdateDto.Release != null && holdDateUpdateDto.Release.Value.Month != releaseDateRules[record.Name])
				{
					validationResults.Add(new HoldDateValidationDto
					{
						IsValid = false,
						Message = $"Release date of '{record.Name}' is not in {GetMonthName(releaseDateRules[record.Name])}"
					});
				}
			}

			foreach (var record in existingRecords)
			{
				if (!holdDatesUpdateDto.Updates.Any(x => x.Name == record.Name))
				{
					validationResults.Add(new HoldDateValidationDto
					{
						IsValid = false,
						Message = $"'{record.Name}' is not found in the update list"
					});
				}
			}
			
			return validationResults;
		}

		public async Task<List<HoldDateDto>> UpdateAllAsync(HoldDatesUpdateDto holdDatesUpdateDto)
		{
			var existingRecords = await dbContext.Set<HoldDate>().ToListAsync();

			foreach(var record in existingRecords)
			{
				var holdDateUpdateDto = holdDatesUpdateDto.Updates.FirstOrDefault(x => x.Name == record.Name);

				if (holdDateUpdateDto == null)
					continue;

				if (holdDateUpdateDto.BeginHold != null)
					record.BeginHold = holdDateUpdateDto.BeginHold;

				if (holdDateUpdateDto.Release != null)
					record.Release = holdDateUpdateDto.Release;
			}

			await dbContext.SaveChangesAsync();

			return mapper.Map<List<HoldDateDto>>(existingRecords);
		}

		private string GetMonthName(int month)
		{
			string[] monthNames = { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };
			return monthNames[month - 1];
		}
	}
}
