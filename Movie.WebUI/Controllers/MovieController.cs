using Movie.WebUI.Models.ViewModel;

namespace Movie.WebUI.Controllers;

public class MovieController(ISender sender,
    ITokenProvider tokenProvider) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Details(int id)
    {
        var query = new GetMovieByIdQuery(id);

        var result = await sender.Send(query);

        if(result == null)
        {
            return NotFound();
        }

        return View(result.Movie);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> AddReview(int id)
    {
        CreateReviewVM createReviewVM = new CreateReviewVM();

        var userId = tokenProvider.GetUserId();

        createReviewVM.ReviewDto.UserId = userId;
        createReviewVM.ReviewDto.MovieId = id;

        return View(createReviewVM);
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddReview(CreateReviewDto reviewDto)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Details", "Movie", new { id = reviewDto.MovieId });
        }

        CreateReviewVM createReviewVM = new CreateReviewVM();
        createReviewVM.ReviewDto = reviewDto;

        return View(createReviewVM);
    }
}
