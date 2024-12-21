namespace Movie.WebUI.Models.ViewModel;

public class CarouselHighlightMoviesVM
{
    public IEnumerable<MovieDto> CarouselMovies { get; set; } 

    public CarouselHighlightMoviesVM()
    {
        CarouselMovies = new List<MovieDto>();
    }
}
