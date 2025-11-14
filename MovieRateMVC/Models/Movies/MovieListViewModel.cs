namespace MovieRateMVC.Models.Movies
{
	public class MovieListViewModel
	{
		public List<MovieListModel> Movies { get; set; }

		public int TotalPages { get; set; }

		public MovieFilterModel Filters { get; set; } = new();
	}
}
