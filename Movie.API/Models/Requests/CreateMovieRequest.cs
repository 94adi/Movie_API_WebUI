namespace Movie.API.Models.Requests;

public record CreateMovieRequest(string Title, 
    float Rating, 
    string Description,
    string ImageUrl,
    int[] Genres,
    IFormFile? Image,
    DateOnly ReleaseDate);