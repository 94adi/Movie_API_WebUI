namespace Movie.API.Models.Requests;

public class UpdateReviewRequest
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}
