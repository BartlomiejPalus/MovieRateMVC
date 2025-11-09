using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Models.Movies;
using MovieRateMVC.Repositories.Interfaces;

namespace MovieRateMVC.Controllers
{
	public class MoviesController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IMovieRepository _movieRepository;

		public MoviesController(UserManager<User> userManager, IMovieRepository movieRepository)
		{
			_userManager = userManager;
			_movieRepository = movieRepository;
		}

		public IActionResult Index()
		{
			return View();
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

			var model = new MovieDetailsModel
			{
				Title = movie.Title,
				Description = movie.Description,
				Genres = movie.Genres.Select(g => g.Name.ToString()).ToList(),
				ReleaseDate = movie.ReleaseDate,
				Director = movie.Director,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add([Bind("Title", "Description", "Genres", 
			"ReleaseDate", "Director")] AddMovieModel model)
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
	}
}
