using Movie.WebUI.Models.ViewModel;

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
        int noOfColumns;

        if(!Int32.TryParse(_config.PageSize, out pageSize))
        {
            pageSize = 9;
        }

        if(!Int32.TryParse(_config.NumberOfColumns, out noOfColumns))
        {
            noOfColumns = 3;
        }

        var result = await _sender.Send(new GetMoviesByPagingQuery(page, pageSize));

        indexMovieVM.NumberOfColumns = noOfColumns;

        indexMovieVM.Result = new PagedResultVM<MovieDto>
        {
            Items = result.MovieDtos.ToList(),
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = result.MovieDtos.Count(),
        };

        if (indexMovieVM.Result.TotalCount <= pageSize)
        {
            indexMovieVM.TotalPages = 1;
        }
        else
        {
            indexMovieVM.TotalPages = (indexMovieVM.Result.TotalCount / pageSize) + 1;
        }

        if (indexMovieVM.NumberOfColumns > indexMovieVM.Result.TotalCount)
        {
            indexMovieVM.NumberOfColumns = indexMovieVM.Result.TotalCount;
            indexMovieVM.NumberOfRows = 1;
        }
        else
        {
            indexMovieVM.NumberOfRows = indexMovieVM.Result.TotalCount / indexMovieVM.NumberOfColumns;

            if (indexMovieVM.Result.TotalCount % indexMovieVM.NumberOfColumns > 0)
            {
                indexMovieVM.NumberOfRows++;
            }
        }

        var moviesCarousel = await _sender.Send(new GetMoviesCarouselQuery());
        indexMovieVM.CarouselHighlightMoviesVM.CarouselMovies = (IList<MovieDto>)moviesCarousel.MovieCarousels;

        return View(indexMovieVM);
    }
}