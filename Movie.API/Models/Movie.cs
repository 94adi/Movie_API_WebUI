namespace Movie.API.Models;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; }

    public float Rating { get; set; }

    public string Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }

    public ICollection<Review> Reviews { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public DateTime CreatedDate {  get; set; }

    public DateTime LatestUpdateDate { get; set; }
}
