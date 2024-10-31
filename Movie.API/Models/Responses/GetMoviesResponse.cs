namespace Movie.API.Models.Responses;

public record GetMoviesResponse
{
    public IEnumerable<Models.Dto.MovieDto> MovieDtos { get; set; } = new List<Models.Dto.MovieDto>();
}