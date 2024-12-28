using Movie.WebUI.Utils;

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
        var viewModel = new DetailsVM();
        var query = new GetMovieByIdQuery(id);

        var totalReviewsCountQuery = new GetReviewsByMovieCountQuery(id);

        var result = await sender.Send(query);

        var totalReviewsCountResult = await sender.Send(totalReviewsCountQuery);

        viewModel.Movie = result.Movie;
        viewModel.ReviewsCount = totalReviewsCountResult.ReviewsCount;

        viewModel.Movie.TrailerUrl = YouTubeHelper.ConvertToEmbedUrl(viewModel.Movie.TrailerUrl);

        if (result == null)
        {
            return NotFound();
        }

        return View(viewModel);
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
    public async Task<IActionResult> Reviews(int movieId, int page = 1)
    {
        var movieReviewsVM = new MovieReviewsVM();

        movieReviewsVM.MovieId = movieId;

        movieReviewsVM.PageNumber = page;

        //TO DO: move to config (separate from index movies)
        int pageSize = 5;

        var totalReviewsCountQuery = new GetReviewsByMovieCountQuery(movieId);

        var query = new GetReviewsByMovieQuery(movieId, page, pageSize);

        var result = await sender.Send(query);

        var totalReviewsCount = await sender.Send(totalReviewsCountQuery);

        var pagedResult = new PagedResultVM<ReviewDto>
        {
            Items = result.ReviewDtos.ToList(),
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalReviewsCount.ReviewsCount
        };

        movieReviewsVM.Result = pagedResult;

        movieReviewsVM.PopulateFields();

        return View(movieReviewsVM);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Review(int reviewId)
    {
        
        return View();
    }

    [Authorize]
    [HttpPost]
    [Route("/API/Movie/{movieId}/Rate/{rating}")]
    public async Task<IActionResult> RateMovie(int movieId, int rating)
    {
        var command = new RateMovieCommand(movieId, rating);

        var result = await sender.Send(command);

        return Json(new { success = result.IsSuccess });
    }
}
