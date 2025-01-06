namespace Movie.WebUI.Models.Dto;

public class CreateReviewDto
{
    public int? Id { get; set; }

    [Required]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string Content { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int MovieId { get; set; }
}
