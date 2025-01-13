namespace Movie.API.Models.Requests;

public class GetMovieUsersRatingsRequest
{
    public List<string> UserIds { get; set; }
}
