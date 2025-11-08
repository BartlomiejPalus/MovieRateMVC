using System.ComponentModel.DataAnnotations;

namespace MovieRateMVC.Enums
{
	public enum MovieGenre
	{
		Action,
		Comedy,
		Drama,
		Horror,
		Thriller,
		[Display(Name ="Sci-Fi")]
		SciFi,
		Fantasy,
		Romance,
		Adventure,
		Other
	}
}
