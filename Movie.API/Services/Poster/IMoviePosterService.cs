namespace Movie.API.Services.Poster;

public interface IMoviePosterService
{
    Task StoreMoviePoster(Models.Movie movie, IFormFile poster);
}
