using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Models;

namespace MovieRateMVC.Controllers
{
	public class AuthController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Login()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login([Bind("Email, Password, RememberMe")] LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid credentials");
				return View(model);
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

			if (result.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "Account locked due to too many failed attempts. Try again later.");
				return View(model);
			}
			
			if (!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Invalid credentials");
				return View(model);
			}

			await _signInManager.SignInAsync(user, model.RememberMe);

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> Register([Bind("Username, Email, Password")] RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			if (await _userManager.FindByEmailAsync(model.Email) != null 
				|| await _userManager.FindByNameAsync(model.Username) != null)
			{
				ModelState.AddModelError(string.Empty, "Username or email address is already taken");
				return View(model);
			}

			var user = new User
			{
				UserName = model.Username,
				Email = model.Email
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, result.Errors.First().Description);
				return View(model);
			}

			return View("Login");
		}
	}
}
