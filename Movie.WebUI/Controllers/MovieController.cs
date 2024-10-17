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
			return View(result.Movie);
		}

        return RedirectToAction("Index", "Movie");

    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromForm] MovieDto model)
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
    [Authorize(Roles = "SuperAdmin")]
    public IActionResult Delete(int movieId) 
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public IActionResult Delete(DeleteMovieDto model) 
    {
        return View();
    }
}
