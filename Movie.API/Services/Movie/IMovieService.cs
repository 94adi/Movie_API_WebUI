namespace Movie.API.Services.Movie;

public interface IMovieService
{
    Task StoreMoviePoster(Models.Movie movie, IFormFile poster);
}
