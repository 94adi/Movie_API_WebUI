namespace Movie.WebUI.Services.Abstractions;

public interface IGenreService
{
    Task<GetGenresResult> GetGenres();
}
