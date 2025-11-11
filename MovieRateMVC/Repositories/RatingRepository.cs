using Microsoft.EntityFrameworkCore;
using MovieRateMVC.Data;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Repositories.Interfaces;

namespace MovieRateMVC.Repositories
{
	public class RatingRepository : IRatingRepository
	{
		private readonly ApplicationDbContext _context;

		public RatingRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Rating rating)
		{
			await _context.Ratings.AddAsync(rating);
			await _context.SaveChangesAsync();
		}

		public async Task<Rating?> GetByMovieAndUserAsync(Guid movieId, string userId)
		{
			return await _context.Ratings
				.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
