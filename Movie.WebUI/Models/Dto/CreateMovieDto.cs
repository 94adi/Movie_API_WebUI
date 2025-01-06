using Movie.WebUI.Utils.Attributes;

namespace Movie.WebUI.Models.Dto;

public class CreateMovieDto
{
    public int? Id { get; set; }
    [Required]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    public IEnumerable<string> SelectedGenres { get; set; }

    public string? ImageUrl { get; set; }

    public string? TrailerUrl {  get; set; }

    public IFormFile? Image { get; set; }

    [ValidReleaseDate]
    public DateOnly ReleaseDate { get; set; }
}