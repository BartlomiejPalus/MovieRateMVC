using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Models.Movies;
using MovieRateMVC.Repositories.Interfaces;

namespace MovieRateMVC.Controllers
{
	public class MoviesController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IMovieRepository _movieRepository;
		private readonly IRatingRepository _ratingRepository;

		public MoviesController(UserManager<User> userManager, IMovieRepository movieRepository,
			IRatingRepository ratingRepository)
		{
			_userManager = userManager;
			_movieRepository = movieRepository;
			_ratingRepository = ratingRepository;
		}

		public async Task<IActionResult> Index(int PageNumber = 1, int PageSize = 10)
		{
			var query = _movieRepository.GetMovies();

			var totalMovies = await query.CountAsync();
			var totalPages = (int)Math.Ceiling(totalMovies / (double)PageSize);
			query = query.Skip((PageNumber - 1) * PageSize).Take(PageSize);

			var movies = await query.Select(m => new MovieListModel
			{
				Id = m.Id,
				Title = m.Title,
				ReleaseDate = m.ReleaseDate
			}).ToListAsync();

			var model = new MovieListViewModel
			{
				Movies = movies,
				CurrentPage = PageNumber,
				TotalPages = totalPages
			};

			return View(model);
		}

		public IActionResult Add()
		{
			var model = new AddMovieModel();
			return View(model);
		}

		public async Task<IActionResult> Details(Guid id)
		{
			var movie = await _movieRepository.GetByIdAsync(id);

			if (movie == null)
				return RedirectToAction("Index", "Home");

			int userRating = 0;
			var user = await _userManager.GetUserAsync(User);

			if (user != null)
			{
				var rating = await _ratingRepository.GetByMovieAndUserAsync(id, user.Id);
				if (rating != null)
					userRating = rating.Mark;
			}
			
			var model = new MovieDetailsModel
			{
				Id = movie.Id,
				Title = movie.Title,
				Description = movie.Description,
				Genres = movie.Genres.Select(g => g.Name.ToString()).ToList(),
				ReleaseDate = movie.ReleaseDate,
				Director = movie.Director,
				UserRating = userRating
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add([Bind("Title, Description, Genres, " +
			"ReleaseDate, Director")] AddMovieModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var genres = await _movieRepository.GetGenresByIdAsync(model.Genres);
			var user = await _userManager.GetUserAsync(User);

			var movie = new Movie
			{
				Title = model.Title,
				Description = model.Description,
				Genres = genres,
				ReleaseDate = model.ReleaseDate,
				Director = model.Director,
				User = user
			};

			await _movieRepository.AddAsync(movie);

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Modify(Guid id)
		{
			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie == null)
				return NotFound();

			var model = new ModifyMovieModel
			{
				Id = movie.Id,
				Title = movie.Title,
				Description = movie.Description,
				Genres = movie.Genres.Select(g => g.Id).ToList(),
				ReleaseDate = movie.ReleaseDate,
				Director = movie.Director
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Modify([Bind("Id, Title, Description, Genres, " +
			"ReleaseDate, Director")] ModifyMovieModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var movie = await _movieRepository.GetByIdAsync(model.Id);
			if (movie == null)
				return NotFound();

			var genres = await _movieRepository.GetGenresByIdAsync(model.Genres);

			movie.Title = model.Title;
			movie.Description = model.Description;
			movie.Genres = genres;
			movie.ReleaseDate = model.ReleaseDate;
			movie.Director = model.Director;

			await _movieRepository.SaveChangesAsync();

			return RedirectToAction("Index", "Movies");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(Guid id)
		{
			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie == null)
				return NotFound();

			var model = new DeleteMovieModel
			{
				Id = movie.Id,
				Title = movie.Title
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie == null)
				return NotFound();

			await _movieRepository.DeleteAsync(movie);

			return RedirectToAction("Index", "Movies");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Rate(Guid id, int rate)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return NotFound();

			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie == null)
				return NotFound();

			var rating = new Rating
			{
				Mark = rate,
				Movie = movie,
				User = user
			};

			await _ratingRepository.AddAsync(rating);

			return RedirectToAction("Details", new { id });
		}
	}
}
