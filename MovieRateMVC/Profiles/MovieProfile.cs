using AutoMapper;
using MovieRateMVC.Data.Entities;
using MovieRateMVC.Models.Movies;

namespace MovieRateMVC.Profiles
{
	public class MovieProfile : Profile
	{
		public MovieProfile()
		{
			CreateMap<Movie, MovieListModel>()
				.ForMember(dest => dest.AverageRating,
				opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(src => src.Mark) : 0));

			CreateMap<Movie, MovieDetailsModel>()
				.ForMember(dest => dest.Genres,
				opt => opt.MapFrom(src => src.Genres.Select(g => g.Name.ToString()).ToList()));

			CreateMap<Movie, ModifyMovieModel>()
				.ForMember(dest => dest.Genres,
				opt => opt.MapFrom(src => src.Genres.Select(g => g.Id).ToList()));

			CreateMap<AddMovieModel, Movie>()
				.ForMember(dest => dest.Genres, opt => opt.Ignore());
		}
	}
}
