namespace Movie.API.Models;

public class Rating
{
    public int Id { get; set; }

    public int RatingValue {  get; set; }

    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    public int MovieId { get; set; }

    public Movie Movie {  get; set; }
}
