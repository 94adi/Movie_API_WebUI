namespace Movie.API.Models.Requests;

public class CreateReviewRequest
{
    public string? Title { get; set; }

    public string? Content { get; set; }

    public float Rating { get; set; }

    public int MovieId { get; set; }

    public string UserId { get; set; }
}
