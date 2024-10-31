namespace Movie.API.Models.Responses;

public class GetGenresResponse
{
    public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
}
