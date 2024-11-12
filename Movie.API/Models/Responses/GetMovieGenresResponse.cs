using Movie.API.Models.Dto;

namespace Movie.API.Models.Responses;

public class GetMovieGenresResponse
{
    public IEnumerable<GenreDto> Genres { get; set; }
}
