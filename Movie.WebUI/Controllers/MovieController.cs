using System.Reflection;

namespace Movie.WebUI.Controllers;

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

        return View(result.Movies);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreateMovieDto model)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateMovieCommand(model);

            var result = await _sender.Send(command);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Movie");
            }
        }

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int movieId)
    {
		var query = new GetMovieByIdQuery(movieId);
        var result = await _sender.Send(query);

        if(result != null && result.Movie != null)
        {
            var updateMovieDto = _mapper.Map<UpdateMovieDto>(result.Movie);
			return View(updateMovieDto);
		}

        return RedirectToAction("Index", "Movie");

    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromForm] UpdateMovieDto model)
    {
        if (ModelState.IsValid)
        {
            var command = new UpdateMovieCommand(model);

            var result = await _sender.Send(command);

            if (result.IsSuccess)
            {
                TempData["success"] = $"The movie {model.Title} was updated successfully";
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

}

