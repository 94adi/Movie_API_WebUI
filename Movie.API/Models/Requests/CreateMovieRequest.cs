namespace Movie.API.Models.Requests;

public record CreateMovieRequest(string Title,
    string Description,
    string SelectedGenres,
    IFormFile? Image,
    string TrailerUrl,
    DateOnly ReleaseDate);