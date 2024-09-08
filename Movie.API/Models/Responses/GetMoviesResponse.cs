namespace Movie.API.Models.Responses;

public record GetMoviesResponse
{
    public IEnumerable<Models.Movie> Movies { get; set; } = new List<Models.Movie>();
}