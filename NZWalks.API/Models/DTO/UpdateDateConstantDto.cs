using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
	public class UpdateDateConstantDto
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Name { get; set; }

		[MaxLength(255)]
		public string? Description { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime Value { get; set; }

		public bool? IsSystemDate { get; set; }
	}
}
