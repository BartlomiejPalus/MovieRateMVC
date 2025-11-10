using Microsoft.AspNetCore.Mvc.Rendering;
using MovieRateMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Models.Movies
{
	public class ModifyMovieModel
	{
		public Guid Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

		[Required]
		[MaxLength(500)]
		public string Description { get; set; }

		[Required]
		[MinLength(1)]
		public List<int> Genres { get; set; } = [];

		public DateOnly? ReleaseDate { get; set; }

		public string? Director { get; set; }

		public IEnumerable<SelectListItem> AllGenres =>
			Enum.GetValues(typeof(MovieGenre))
				.Cast<MovieGenre>()
				.Select(g => new SelectListItem
				{
					Value = ((int)g + 1).ToString(),
					Text = g.ToString()
				});
	}
}
