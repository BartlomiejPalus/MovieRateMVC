using MovieRateMVC.Data.Entities;

namespace MovieRateMVC.Repositories.Interfaces
{
	public interface IMovieRepository
	{
		Task AddAsync(Movie movie);
		Task<Movie?> GetByIdAsync(Guid id);
		IQueryable<Movie> GetMovies();
		Task DeleteAsync(Movie movie);
		Task<List<Genre>> GetGenresByIdAsync(List<int> genres);
		Task SaveChangesAsync();
	}
}