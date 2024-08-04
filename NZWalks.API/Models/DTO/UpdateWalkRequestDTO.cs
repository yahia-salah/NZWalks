using NZWalks.API.Models.Domains;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
	public class UpdateWalkRequestDTO
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		[MaxLength(1000)]
		public string Description { get; set; }
		[MaxLength(255)]
		public string? ImageUrl { get; set; }
		[Required]
		[Range(0, 50)]
		public double LengthInKm { get; set; }
		[Required]
		public Guid RegionId { get; set; }
		[Required]
		public Guid DifficultyId { get; set; }
	}
}
