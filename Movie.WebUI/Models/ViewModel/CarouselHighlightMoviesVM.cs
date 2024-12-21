namespace Movie.WebUI.Models.ViewModel;

public class CarouselHighlightMoviesVM
{
    public IList<MovieDto> CarouselMovies { get; set; } 

    public CarouselHighlightMoviesVM()
    {
        CarouselMovies = new List<MovieDto>();
    }
}
