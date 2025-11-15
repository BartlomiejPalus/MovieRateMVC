using Microsoft.IdentityModel.Tokens;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Enums;
using MovieRateMVC.Models.Movies;
using MovieRateMVC.Services.Interfaces;

namespace MovieRateMVC.Services
{
	public class MoviesService : IMoviesService
	{
		public IQueryable<Movie> ApplyFilters(IQueryable<Movie> query, MovieFilterModel filters)
		{
			if (!filters.Title.IsNullOrEmpty())
				query = query.Where(q => q.Title.Contains(filters.Title));

			if (!filters.Director.IsNullOrEmpty())
				query = query.Where(q => q.Director.Contains(filters.Director));

			if (filters.Genre != null)
				query = query.Where(q => q.Genres.Any(g => g.Name == filters.Genre));

			return query;
		}

		public IQueryable<Movie> ApplySorting(IQueryable<Movie> query, MovieFilterModel filters)
		{
			switch (filters.SortBy)
			{
				case MovieSortBy.CreatedAt:
					query = filters.SortDesc ? query.OrderByDescending(q => q.CreatedAt)
						: query.OrderBy(q => q.CreatedAt);
					break;
				case MovieSortBy.Title:
					query = filters.SortDesc ? query.OrderByDescending(q => q.Title)
						: query.OrderBy(q => q.Title);
					break;
				case MovieSortBy.ReleaseDate:
					query = query.Where(q => q.ReleaseDate != null);
					query = filters.SortDesc ? query.OrderByDescending(q => q.ReleaseDate)
						: query.OrderBy(q => q.ReleaseDate);
					break;
				case MovieSortBy.Rating:
					query = query.Where(q => q.Ratings.Any());
					query = filters.SortDesc ? query.OrderByDescending(q => q.Ratings.Average(q => q.Mark))
						: query.OrderBy(q => q.Ratings.Average(q => q.Mark));
					break;
			}

			return query;
		}

		public IQueryable<Movie> ApplyPagination(IQueryable<Movie> query, MovieFilterModel filters)
		{
			query = query.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize);

			return query;
		}
	}
}
