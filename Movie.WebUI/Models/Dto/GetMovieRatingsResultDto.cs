namespace Movie.WebUI.Models.Dto;

public class GetMovieRatingsResultDto
{
    public IEnumerable<Models.Dto.RatingDto> Ratings { get; set; }
}
