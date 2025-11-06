using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Data.Entities
{
	public class Rating
	{
		public Guid Id { get; set; }

		[Range(1, 10)]
		public int Mark { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public Guid MovieId { get; set; }
		public Movie Movie { get; set; }

		public string? UserId { get; set; }
		public User? User { get; set; }
	}
}
