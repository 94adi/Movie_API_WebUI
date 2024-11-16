namespace Movie.WebUI.Models.ViewModel
{
    public class IndexMovieVM
    {
        public IEnumerable<CarouselItemVM> CarouselItems { get; set; }

        public int PageNumber { get;set; }

        public int TotalPages { get; set; }

        public PagedResultVM<Models.Dto.MovieDto> Result { get; set; }

        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
    }
}
