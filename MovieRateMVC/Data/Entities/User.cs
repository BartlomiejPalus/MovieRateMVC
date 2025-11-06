using Microsoft.AspNetCore.Identity;

namespace MovieRateMVC.Data.Entities
{
	public class User : IdentityUser
	{
		public ICollection<Movie> Movies { get; set; } = [];
		public ICollection<Rating> Ratings { get; set; } = [];
		public ICollection<Review> Reviews { get; set; } = [];
	}
}
