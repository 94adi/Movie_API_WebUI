using Movie.WebUI.Utils.Attributes;

namespace Movie.WebUI.Models.Dto;

public class UpdateMovieDto
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; }

    public List<GenreDto>? Genres { get; set; }

    [Required]
    [Display(Name = "Genre(s)")]
    public IEnumerable<string> SelectedGenres { get; set; }

    public string? ImageUrl { get; set; }

    public IFormFile? Image { get; set; }

    public string? TrailerUrl { get; set; }

    [Required]
    [ValidReleaseDate]
    public DateOnly ReleaseDate { get; set; }

    public DateTime LatestUpdateDate { get; set; }
}
