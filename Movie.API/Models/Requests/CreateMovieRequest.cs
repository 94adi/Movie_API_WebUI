namespace Movie.API.Models.Requests;

public record CreateMovieRequest(string Title, 
    float Rating, 
    string Description,
    string ImageUrl,
    string SelectedGenres,
    IFormFile? Image,
    DateOnly ReleaseDate);