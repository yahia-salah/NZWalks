using AutoMapper;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
	public class AutoMapperProfiles:Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<RegionDTO,Region>()
				.ForMember(des=>des.ImageUrl,opt=>opt.MapFrom(src=>src.ImageUrl))
				.ReverseMap(); // for properties which have same name in src/des, no need to specify explicitly
			CreateMap<AddRegionRequestDTO,Region>().ReverseMap();
			CreateMap<UpdateRegionRequestDTO,Region>().ReverseMap();
			CreateMap<WalkDTO, Walk>().ReverseMap();
			CreateMap<AddWalkRequestDTO, Walk>().ReverseMap();
			CreateMap<UpdateWalkRequestDTO, Walk>().ReverseMap();
			CreateMap<DifficultyDTO, Difficulty>().ReverseMap();
		}
	}
}
