using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
	public class AddRegionRequestDTO
	{
		[Required(ErrorMessage ="Code is required")]
		[MaxLength(3,ErrorMessage ="Code can't exceed 3 characters")]
		[MinLength(2,ErrorMessage ="Code must be at least 2 characters")]
		public string Code { get; set; }
		[Required(ErrorMessage ="Name is required")]
		[MaxLength(50,ErrorMessage ="Name can't exceed 50 characters")]
		public string Name { get; set; }
		[MaxLength(255,ErrorMessage ="ImageUrl can't exceed 255 characters")]
		public string? ImageUrl { get; set; }
	}
}
