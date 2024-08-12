using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Data
{
	public class NZWalksDbContext:DbContext
	{
		public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options):base(options)
		{
		}

		public DbSet<Walk> Walks { get; set; }
		public DbSet<Region> Regions { get; set; }
		public DbSet<Difficulty> Difficulties { get; set; }
		public DbSet<Image> Images { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed data for difficulties
			// Easy, Medium, Hard
			var difficulties = new List<Difficulty>()
			{
				new Difficulty(){Id=Guid.Parse("250713a7-58ab-43a9-8256-0574718edde3"),Name="Easy"},
				new Difficulty(){Id=Guid.Parse("51d91533-e380-40bd-b0b9-2ab226885ef2"),Name="Medium"},
				new Difficulty(){Id=Guid.Parse("bbbc3995-0041-440a-bc04-a327c9b4fc89"),Name="Hard"}
			};

			modelBuilder.Entity<Difficulty>().HasData(difficulties);

			// Seed data for regions
			var regions = new List<Region>()
			{
				new Region(){Id=Guid.Parse("f1b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),Name="Northland",ImageUrl="https://www.doc.govt.nz/globalassets/images/places/northland/northland-landscape-1.jpg",Code="NO"},
				new Region(){Id=Guid.Parse("f2b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),Name="Auckland",ImageUrl="https://www.doc.govt.nz/globalassets/images/places/auckland/auckland-landscape-1.jpg",Code="AU"},
				new Region(){Id=Guid.Parse("f3b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),Name="Waikato",ImageUrl="https://www.doc.govt.nz/globalassets/images/places/waikato/waikato-landscape-1.jpg",Code="WK"},
				new Region(){Id=Guid.Parse("f4b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),Name="Bay of Plenty",ImageUrl="https://www.doc.govt.nz/globalassets/images/places/bay-of-plenty/bay-of-plenty-landscape-1.jpg",Code="BP"},
				new Region(){Id=Guid.Parse("f5b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),Name="Gisborne",ImageUrl="https://www.doc.govt.nz/globalassets/images/places/gisborne/gisborne-landscape-1.jpg",Code="GI"},
				new Region(){Id=Guid.Parse("f6b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),Name="Hawke's Bay",ImageUrl="https://www.doc.govt.nz/globalassets/images/places/hawkes-bay/hawkes-bay-landscape-1.jpg",Code="HB"},
			};
			modelBuilder.Entity<Region>().HasData(regions);
		}
	}
}
