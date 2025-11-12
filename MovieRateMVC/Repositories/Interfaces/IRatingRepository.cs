using MovieRateMVC.Data.Entities;

namespace MovieRateMVC.Repositories.Interfaces
{
	public interface IRatingRepository
	{
		Task AddAsync(Rating rating);
		Task<Rating?> GetByMovieAndUserAsync(Guid movieId, string userId);
		Task<double> GetAverageRatingByMovieIdAsync(Guid id);
		Task SaveChangesAsync();
	}
}