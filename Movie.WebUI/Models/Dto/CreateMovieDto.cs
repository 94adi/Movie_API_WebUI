using Movie.WebUI.Utils.Attributes;

namespace Movie.WebUI.Models.Dto;

public class CreateMovieDto
{
    public int? Id { get; set; }
    [Required]
    public string Title { get; set; }

    [Required]
    [Range(1.0, 10.0)]
    public float Rating { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    public string? ImageUrl { get; set; }

    public IFormFile? Image { get; set; }

    [ValidReleaseDate]
    public DateOnly ReleaseDate { get; set; }
}