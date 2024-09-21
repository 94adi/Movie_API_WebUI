namespace Movie.WebUI.Services.Abstractions;

public interface IMovieService
{
    IEnumerable<MovieDto> GetMovies(int pageNumber, int pageSize);

	Task<GetAllMoviesResultDto> GetAllMovies();

    Task<CreateMovieResultDto> CreateMovie(CreateMovieDto movieDto);
}
