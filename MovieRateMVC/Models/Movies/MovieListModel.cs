namespace MovieRateMVC.Models.Movies
{
	public class MovieListModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public DateOnly? ReleaseDate { get; set; }

		public double AverageRating { get; set; }
	}
}
