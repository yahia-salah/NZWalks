﻿using Microsoft.EntityFrameworkCore;
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
	}
}
