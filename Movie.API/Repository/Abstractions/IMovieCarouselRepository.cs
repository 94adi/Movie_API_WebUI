namespace Movie.API.Repository.Abstractions;

public interface IMovieCarouselRepository : IRepository<MovieCarousel>
{
    Task<Movie.API.Models.MovieCarousel> UpdateAsync(Movie.API.Models.MovieCarousel movieCarousel);
}
