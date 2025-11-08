using MovieRateMVC.Enums;

namespace MovieRateMVC.Data.Entities
{
	public class Genre
	{
		public int Id { get; set; }
		public MovieGenre Name { get; set; }

		public ICollection<Movie> Movies { get; set; } = [];
	}
}
