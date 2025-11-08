using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Enums;

namespace MovieRateMVC.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Movie> Movies { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Review> Reviews { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Genre>()
				.Property(g => g.Name)
				.HasConversion<String>();

			var genres = Enum.GetValues(typeof(MovieGenre))
				.Cast<MovieGenre>()
				.Select((name, index) => new Genre
				{
					Id = index + 1,
					Name = name
				})
				.ToList();

			builder.Entity<Genre>().HasData(genres);
		}
	}
}
