using MovieRateMVC.Data.Entities;
using MovieRateMVC.Models.Movies;

namespace MovieRateMVC.Services.Interfaces
{
	public interface IMoviesService
	{
		IQueryable<Movie> ApplyFilters(IQueryable<Movie> query, MovieFilterModel filters);
		IQueryable<Movie> ApplyPagination(IQueryable<Movie> query, MovieFilterModel filters);
		IQueryable<Movie> ApplySorting(IQueryable<Movie> query, MovieFilterModel filters);
	}
}