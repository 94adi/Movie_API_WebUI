namespace Movie.API.Services.Movie;

public interface IMovieService
{
    Task StoreMoviePoster(Models.Movie movie, IFormFile poster);

    Task<Models.Movie> GetByIdAsync(int id, bool includeGenres = false);

    Task RemoveMovieGenres(int movieId);

    Task AddMovieGenres(int movieId, IEnumerable<int> genreIds);

    Task<int> GetMoviesCount();

    Task<IList<Models.Movie>> GetMoviesWithGenres(int pageNumber = 1, int pageSize = 0);

    Task AddPosterUrls(IEnumerable<Models.Movie> movies);
}
