namespace Movie.WebUI.Controllers;

public class HomeController : Controller
{

    private readonly MovieAppConfig _config;
    private readonly ISender _sender;

    public HomeController(IOptions<MovieAppConfig> configOptions,
        ISender sender)
    {
        _config = configOptions.Value;
        _sender = sender;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        IndexMovieVM indexMovieVM = new IndexMovieVM();
        indexMovieVM.PageNumber = page;

        int pageSize;

        if(!Int32.TryParse(_config.PageSize, out pageSize))
        {
            pageSize = 6;
        }

        var result = await _sender.Send(new GetMoviesByPagingQuery(page, pageSize));

        var totalMoviesCount = await _sender.Send(new GetMoviesCountQuery());

        indexMovieVM.Result = new PagedResultVM<MovieDto>
        {
            Items = result.MovieDtos.ToList(),
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalMoviesCount.MoviesCount,
        };

        indexMovieVM.PopulateFields();

        var moviesCarousel = await _sender.Send(new GetMoviesCarouselQuery());
        indexMovieVM.CarouselHighlightMoviesVM.CarouselMovies = (IList<MovieDto>)moviesCarousel.MovieCarousels;

        return View(indexMovieVM);
    }
}