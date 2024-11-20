using Movie.WebUI.Models.ViewModel;
using Movie.WebUI.Services.Handlers.Genres.Queries;

namespace Movie.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class MovieController : Controller
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public MovieController(IMapper mapper,
        ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<MovieDto> moviesList = new List<MovieDto>();

        var result = await _sender.Send(new GetAllMoviesQuery());

        return View(result.MovieDtos);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var createMovieVM = new CreateMovieVM();

        var genreQueryResult = await _sender.Send(new GetAllGenresQuery());

        createMovieVM.GenreOptions = genreQueryResult.GenreDtos.Select(g => new SelectListItem
        {
            Value = g.Id.ToString(),
            Text = g.Name,
        }).ToList();

        return View(createMovieVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreateMovieVM model)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateMovieCommand(model.MovieDto);

            var result = await _sender.Send(command);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Movie");
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int movieId)
    {
		var query = new GetMovieByIdQuery(movieId);
        var result = await _sender.Send(query);

        if(result != null && result.Movie != null)
        {
            var updateMovieDto = _mapper.Map<UpdateMovieDto>(result.Movie);
            var updateMovieVM = new UpdateMovieVM();

            updateMovieVM.MovieDto = updateMovieDto;

            var allGenresQuery = new GetAllGenresQuery();

            var resultGenres = await _sender.Send(allGenresQuery);

            var slelectedOptionIds = updateMovieDto?.Genres?.Select(g => g.Id);

            bool hasSelectedGenres = ((slelectedOptionIds != null) && (slelectedOptionIds.Count() > 0));

            updateMovieVM.GenreOptions = resultGenres.GenreDtos.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name,
                Selected = hasSelectedGenres ? slelectedOptionIds.Contains(g.Id) : false
            }).ToList();

            return View(updateMovieVM);
		}

        return RedirectToAction("Index", "Movie");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromForm] UpdateMovieVM model)
    {
        if (ModelState.IsValid)
        {
            var command = new UpdateMovieCommand(model.MovieDto);

            var result = await _sender.Send(command);

            if (result.IsSuccess)
            {
                TempData["success"] = $"The movie {model.MovieDto.Title} was updated successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        TempData["error"] = "Error encountered";
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int movieId)
    {
        var query = new GetMovieByIdQuery(movieId);
        var result = await _sender.Send(query);

        if (result != null && result.Movie != null)
        {
            return View(result.Movie);
        }

        //TO DO: make no permission page
        return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(MovieDto movie) 
    {
        if((movie == null) || (movie?.Id < 0))
        {
            ModelState.AddModelError("Delete", "Could not perform operation");
            return RedirectToAction("Index", "Movie");
        }

        var command = new DeleteMovieCommand(movie.Id);

        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            TempData["success"] = $"The movie {movie.Title} was deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = "Error encountered";
        return View(movie);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CarouselMovies()
    {
        var viewModel = new CarouselItemVM();

        var result = await _sender.Send(new GetAllMoviesQuery());

        var movies = result.MovieDtos;

        viewModel.MovieOptions = movies.Select(m => new SelectListItem
        {
            Text = m.Title,
            Value = Convert.ToString(m.Id)
        }).ToList();

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CarouselMovies([FromForm] CarouselItemVM carouselItemVM)
    {
        if (ModelState.IsValid)
        {
            var command = new UpdateMovieCarouselCommand(carouselItemVM.SelectedMovieOptions);
            var result = await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        var allMoviesQueryResult = await _sender.Send(new GetAllMoviesQuery());

        var moviesResult = allMoviesQueryResult.MovieDtos;

        carouselItemVM.MovieOptions = moviesResult.Select(m => new SelectListItem
        {
            Text = m.Title,
            Value = Convert.ToString(m.Id)
        }).ToList();

        return View(carouselItemVM);
    }

}