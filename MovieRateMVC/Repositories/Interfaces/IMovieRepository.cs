using MovieRateMVC.Data.Entities;

namespace MovieRateMVC.Repositories.Interfaces
{
	public interface IMovieRepository
	{
		Task AddAsync(Movie movie);
		List<Genre> GetGenresById(List<int> genres);
	}
}