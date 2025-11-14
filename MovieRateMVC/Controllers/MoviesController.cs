using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Enums;
using MovieRateMVC.Models.Movies;
using MovieRateMVC.Repositories.Interfaces;
using MovieRateMVC.Services.Interfaces;

namespace MovieRateMVC.Controllers
{
	public class MoviesController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IMovieRepository _movieRepository;
		private readonly IRatingRepository _ratingRepository;
		private readonly IMoviesService _moviesService;
		private readonly IMapper _mapper;

		public MoviesController(UserManager<User> userManager, IMovieRepository movieRepository,
			IRatingRepository ratingRepository, IMoviesService moviesService, IMapper mapper)
		{
			_userManager = userManager;
			_movieRepository = movieRepository;
			_ratingRepository = ratingRepository;
			_moviesService = moviesService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> Index([FromQuery] MovieFilterModel filters)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var query = _movieRepository.GetMovies();

			query = _moviesService.ApplyFilters(query, filters);

			query = _moviesService.ApplySorting(query, filters);

			var totalMovies = await query.CountAsync();
			var totalPages = (int)Math.Ceiling(totalMovies / (double)filters.PageSize);

			query = _moviesService.ApplyPagination(query, filters);

			var movies = await query.Select(m => _mapper.Map<MovieListModel>(m)).ToListAsync();

			var model = new MovieListViewModel
			{
				Movies = movies,
				TotalPages = totalPages,
				Filters = filters
			};

			return View(model);
		}

		[HttpGet]
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

			var avgRating = await _ratingRepository.GetAverageRatingByMovieIdAsync(id);

			var model = _mapper.Map<MovieDetailsModel>(movie);
			model.UserRating = userRating;
			model.AverageRating = avgRating;

			return View(model);
		}

		[HttpGet]
		[Authorize(Roles = nameof(UserRoles.Admin))]
		public IActionResult Add()
		{
			var model = new AddMovieModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = nameof(UserRoles.Admin))]
		public async Task<IActionResult> Add([Bind("Title, Description, Genres, " +
			"ReleaseDate, Director")] AddMovieModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var genres = await _movieRepository.GetGenresByIdAsync(model.Genres);
			var user = await _userManager.GetUserAsync(User);

			var movie = _mapper.Map<Movie>(model);
			movie.Genres = genres;
			movie.User = user;

			await _movieRepository.AddAsync(movie);

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Authorize(Roles = nameof(UserRoles.Admin))]
		public async Task<IActionResult> Modify(Guid id)
		{
			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie == null)
				return NotFound();

			var model = _mapper.Map<ModifyMovieModel>(movie);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = nameof(UserRoles.Admin))]
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
		[Authorize(Roles = nameof(UserRoles.Admin))]
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
		[ValidateAntiForgeryToken]
		[Authorize(Roles = nameof(UserRoles.Admin))]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie == null)
				return NotFound();

			await _movieRepository.DeleteAsync(movie);

			return RedirectToAction("Index", "Movies");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Rate(Guid id, int rate)
		{
			if (rate < 1 || rate > 5)
				return BadRequest("Invalid rate value.");

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return NotFound();
			
			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie == null)
				return NotFound();

			var rating = await _ratingRepository.GetByMovieAndUserAsync(id, user.Id);
			
			if (rating == null)
			{
				rating = new Rating
				{
					Mark = rate,
					Movie = movie,
					User = user
				};

				await _ratingRepository.AddAsync(rating);
			}
			else
			{
				rating.Mark = rate;
				await _ratingRepository.SaveChangesAsync();
			}

			return RedirectToAction("Details", new { id });
		}
	}
}
