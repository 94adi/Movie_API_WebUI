namespace Movie.WebUI.Models.ViewModel
{
    public class IndexMovieVM : PagedBaseVM<Models.Dto.MovieDto>
    {
        public CarouselHighlightMoviesVM CarouselHighlightMoviesVM { get; set; }

        public int NumberOfColumns { get; set; }

        public int NumberOfRows { get; set; }

        public IndexMovieVM()
        {
            CarouselHighlightMoviesVM = new CarouselHighlightMoviesVM();
        }
    }
}
