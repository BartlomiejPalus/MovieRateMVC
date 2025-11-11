namespace MovieRateMVC.Models.Movies
{
	public class MovieDetailsModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public List<string> Genres { get; set; } = [];

		public DateOnly? ReleaseDate { get; set; }
		
		public string? Director { get; set; }
	}
}
