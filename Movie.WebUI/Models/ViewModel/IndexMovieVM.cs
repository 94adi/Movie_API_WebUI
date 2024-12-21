namespace Movie.WebUI.Models.ViewModel
{
    public class IndexMovieVM
    {
        public int PageNumber { get;set; }

        public int TotalPages { get; set; }

        public PagedResultVM<Models.Dto.MovieDto> Result { get; set; }

        public CarouselHighlightMoviesVM CarouselHighlightMoviesVM { get; set; }

        public int NumberOfColumns { get; set; }

        public int NumberOfRows { get; set; }

        public IndexMovieVM()
        {
            CarouselHighlightMoviesVM = new CarouselHighlightMoviesVM();
        }
    }
}
