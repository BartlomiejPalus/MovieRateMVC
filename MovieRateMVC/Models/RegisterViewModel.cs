using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Models
{
	public class RegisterViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
