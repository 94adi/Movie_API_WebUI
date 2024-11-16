namespace Movie.API.Models.Dto;

public class MovieDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public float Rating { get; set; }

    public string Description { get; set; }

    public List<GenreDto> Genres { get; set; } = new();

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }

    public string? TrailerUrl { get; set; }

    public ICollection<int> Reviews { get; set; } = new List<int>();

    public DateOnly ReleaseDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LatestUpdateDate { get; set; }
}
