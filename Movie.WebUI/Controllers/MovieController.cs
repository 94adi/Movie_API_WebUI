using Movie.WebUI.Models.ViewModel;
using Movie.WebUI.Services.Handlers.Reviews.Commands;
using Movie.WebUI.Services.Handlers.Reviews.Queries;

namespace Movie.WebUI.Controllers;

public class MovieController(ISender sender,
    ITokenProvider tokenProvider,
    IReviewService reviewService) : Controller
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
            var command = new AddReviewCommand(reviewDto);

            var result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return RedirectToAction("Details", "Movie", new { id = reviewDto.MovieId });
            }
            else
            {
                ModelState.AddModelError("Review", "Could not add review");
            }
        }

        CreateReviewVM createReviewVM = new CreateReviewVM();
        createReviewVM.ReviewDto = reviewDto;

        return View(createReviewVM);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Reviews(int movieId)
    {
        var query = new GetReviewsByMovieQuery(movieId);

        var result = await sender.Send(query);

        var reviews = result.ReviewDtos;
        return View(reviews);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Review(int reviewId)
    {
        
        return View();
    }
}
