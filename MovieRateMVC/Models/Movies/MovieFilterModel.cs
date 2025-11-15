using MovieRateMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Models.Movies
{
	public class MovieFilterModel
	{
		[MaxLength(50)]
		public string? Title { get; set; }

		[MaxLength(50)]
		public string? Director { get; set; }

		public MovieGenre? Genre { get; set; }

		public MovieSortBy SortBy { get; set; } = MovieSortBy.CreatedAt;

		public bool SortDesc { get; set; } = false;
		
		[Range(1, int.MaxValue)]
		public int PageNumber { get; set; } = 1;

		[AllowedValues(20, 40, 60)]
		public int PageSize { get; set; } = 20;
	}
}
