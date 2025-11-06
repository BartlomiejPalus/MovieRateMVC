using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Data.Entities
{
	public class Movie
	{
		public Guid Id { get; set; }

		[MaxLength(50)]
		public string Title { get; set; }
		
		[MaxLength(500)]
		public string Description { get; set; }
		public DateOnly? ReleaseDate { get; set; }

		[MaxLength(100)]
		public string? Director { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public string? UserId { get; set; }
		public User? User { get; set; }

		public ICollection<Genre> Genres { get; set; } = [];
		public ICollection<Rating> Ratings { get; set; } = [];
		public ICollection<Review> Reviews { get; set; } = [];
	}
}
