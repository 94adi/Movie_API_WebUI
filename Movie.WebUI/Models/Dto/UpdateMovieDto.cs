namespace Movie.WebUI.Models.Dto;

public class UpdateMovieDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<GenreDto>? Genres { get; set; }

    public IEnumerable<string> SelectedGenres { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }

    public IFormFile? Image { get; set; }

    public string TrailerUrl { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LatestUpdateDate { get; set; }
}
