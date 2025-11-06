using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRateMVC.Data.Entities
{
	public class Review
	{
		public Guid Id { get; set; }

		[MaxLength(100)]
		public string Title { get; set; }

		[MaxLength(500)]
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public Guid MovieId { get; set; }
		public Movie Movie { get; set; }

		public string? UserId { get; set; }
		public User? User { get; set; }
	}
}
