namespace Movie.API.Models;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; }

    public decimal Rating { get; set; }

    public string Description { get; set; }

    public List<MovieGenre> MovieGenres { get; set; } = new();

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public string TrailerUrl { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public DateTime CreatedDate {  get; set; }

    public DateTime LatestUpdateDate { get; set; }
}
