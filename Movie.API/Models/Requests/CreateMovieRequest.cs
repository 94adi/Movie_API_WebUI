namespace Movie.API.Models.Requests;

public record CreateMovieRequest(string Title, 
    float Rating, 
    string Description,
    string ImageUrl,
    IFormFile? Image,
    DateOnly ReleaseDate);