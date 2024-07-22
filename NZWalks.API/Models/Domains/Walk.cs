namespace NZWalks.API.Models.Domains
{
	public class Walk
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string? ImageUrl { get; set; }
		public double LengthInKm { get; set; }

		public Guid RegionId { get; set; }
		public Region Region { get; set; }
		public Guid DifficultyId { get; set; }
		public Difficulty Difficulty { get; set; }
	}
}
