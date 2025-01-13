namespace Movie.WebUI.Models.Dto;

public class GetMovieRatingsRequestDto
{
    public GetMovieRatingsRequestDto(IEnumerable<string> userIds)
    {
        UserIds = userIds;
    }
    public IEnumerable<string> UserIds { get; set; }
}
