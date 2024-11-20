namespace Movie.API.Models;

public class MovieCarousel
{
    public int Id { get; set; }
    public int MovieId {  get; set; }
    public Movie Movie { get; set; }
}
