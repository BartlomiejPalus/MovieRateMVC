using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Models
{
	public class LoginViewModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public bool RememberMe { get; set; } = false;
	}
}
