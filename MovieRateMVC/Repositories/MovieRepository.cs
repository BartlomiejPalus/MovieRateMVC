using Microsoft.EntityFrameworkCore;
using MovieRateMVC.Data;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Repositories.Interfaces;

namespace MovieRateMVC.Repositories
{
	public class MovieRepository : IMovieRepository
	{
		private readonly ApplicationDbContext _context;

		public MovieRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Movie movie)
		{
			await _context.Movies.AddAsync(movie);
			await _context.SaveChangesAsync();
		}

		public async Task<Movie?> GetByIdAsync(Guid id)
		{
			return await _context.Movies
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public IQueryable<Movie> GetMovies()
		{
			return _context.Movies
				.Include(m => m.Genres)
				.Include(m => m.Ratings)
				.AsQueryable();
		}

		public async Task DeleteAsync(Movie movie)
		{
			_context.Movies.Remove(movie);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Genre>> GetGenresByIdAsync(List<int> genres)
		{
			return await _context.Genres.Where(g => genres.Contains(g.Id)).ToListAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
