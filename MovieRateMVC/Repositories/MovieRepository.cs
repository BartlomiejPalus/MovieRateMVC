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

		public List<Genre> GetGenresById(List<int> genres)
		{
			return _context.Genres.Where(g => genres.Contains(g.Id)).ToList();
		}
	}
}
