namespace Movie.API.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Rating { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate {  get; set; }
    public DateTime LatestUpdateDate { get; set; }
}
