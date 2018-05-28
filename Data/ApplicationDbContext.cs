using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mapa.Models;

namespace Mapa.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// shadow properties
			modelBuilder.Entity<POI>().Property<DateTime>("UpdatedTimestamp");

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<POITag>()
				.HasKey(bc => new { bc.POIId, bc.TagId });

			modelBuilder.Entity<POITag>()
				.HasOne(bc => bc.POI)
				.WithMany(b => b.DefinedTags)
				.HasForeignKey(bc => bc.POIId);

			modelBuilder.Entity<POITag>()
				.HasOne(bc => bc.Tag)
				.WithMany(c => c.POIs)
				.HasForeignKey(bc => bc.TagId);
		}

		public override int SaveChanges()
		{
			//ChangeTracker.DetectChanges();

			updateUpdatedProperty<POI>();

			return base.SaveChanges();
		}

		private void updateUpdatedProperty<T>() where T : class
		{
			var modifiedSourceInfo =
				ChangeTracker.Entries<T>()
					.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

			foreach (var entry in modifiedSourceInfo)
			{
				entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
			}
		}

		public DbSet<POI> POIs { get; set; }

		public DbSet<Mapa.Models.Tag> Tags { get; set; }
		//public DbSet<POITag> POITags { get; set; }
	}
}
