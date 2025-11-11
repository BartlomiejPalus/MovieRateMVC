using MovieRateMVC.Data.Entities;

namespace MovieRateMVC.Repositories.Interfaces
{
	public interface IRatingRepository
	{
		Task AddAsync(Rating rating);
	}
}