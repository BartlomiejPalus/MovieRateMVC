using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Models.Movies
{
	public class MovieDetailsModel
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public List<string> Genres { get; set; } = [];

		public DateOnly? ReleaseDate { get; set; }
		
		public string? Director { get; set; }
	}
}
