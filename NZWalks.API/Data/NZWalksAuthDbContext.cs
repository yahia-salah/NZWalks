using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
	public class NZWalksAuthDbContext:IdentityDbContext
	{
		public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options):base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed data for roles
			var readerRoleId = "ebc4bece-e072-4416-8693-9f6e09890690";
			var writerRoleId = "5cf07d76-236d-459b-9688-6c88d6f5b6fe";

			var roles = new List<IdentityRole>()
			{
				new IdentityRole(){Id=readerRoleId,ConcurrencyStamp=readerRoleId,Name="Reader",NormalizedName="READER"},
				new IdentityRole(){Id=writerRoleId,ConcurrencyStamp=writerRoleId,Name="Writer",NormalizedName="WRITER"}
			};

			modelBuilder.Entity<IdentityRole>().HasData(roles);
		}
	}
}
