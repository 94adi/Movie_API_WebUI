namespace Movie.API.Models.Dto;

public class RatingDto
{
    public string UserId { get; set; }
    public int RatingValue { get; set; }
    public int MovieId { get; set; }
}
