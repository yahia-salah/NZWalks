namespace NZWalks.API.Models.DTO
{
    public class DateConstantDto
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public DateTime Value { get; set; }
		public bool? IsSystemDate { get; set; }
	}
}
