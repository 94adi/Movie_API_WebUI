namespace Movie.API.Models.Requests;

public record CreateMovieRequest(string Title, 
    float Rating, 
    string Description, 
    DateOnly ReleaseDate);