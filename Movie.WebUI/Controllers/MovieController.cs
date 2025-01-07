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

        var getMovieByIdQuery = new GetMovieByIdQuery(id);
        var totalReviewsCountQuery = new GetReviewsByMovieCountQuery(id);
        var movieScoreQuery = new GetMovieRatingQuery(id);

        var result = await sender.Send(getMovieByIdQuery);
        var totalReviewsCountResult = await sender.Send(totalReviewsCountQuery);
        var movieScore = await sender.Send(movieScoreQuery);

        if (User != null && User.Identity.IsAuthenticated)
        {
            var getMovieRatingByUserQuery = new GetMovieRatingByUserQuery(id);
            var userMovieRating = await sender.Send(getMovieRatingByUserQuery);
            viewModel.UserRating = userMovieRating?.Rating?.RatingValue;
        }
        else
        {
            viewModel.UserRating = null;
        }

        viewModel.Movie = result.Movie;
        viewModel.ReviewsCount = totalReviewsCountResult.ReviewsCount;
        

        viewModel.Movie.TrailerUrl = YouTubeHelper.ConvertToEmbedUrl(viewModel.Movie.TrailerUrl);

        viewModel.Movie.FinalScore = movieScore.Rating.HasValue ? movieScore.Rating.Value : 0;

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
        createReviewVM.RatingDto.UserId = userId;
        createReviewVM.RatingDto.MovieId = id;

        var getMovieQuery = new GetMovieByIdQuery(id);
        var movieResult = await sender.Send(getMovieQuery);

        createReviewVM.PageTitle = $"Add review for: {movieResult.Movie.Title}";

        return View(createReviewVM);
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddReview([FromForm] CreateReviewVM createReviewVM)
    {
        if (ModelState.IsValid)
        {
            var addReviewCommand = new AddReviewCommand(createReviewVM.ReviewDto);

            var rateMovieCommand = new RateMovieCommand(createReviewVM.RatingDto.MovieId,createReviewVM.RatingDto.RatingValue);

            var resultRateMovie = await sender.Send(rateMovieCommand);

            var resultAddReview = await sender.Send(addReviewCommand);

            if (resultAddReview.IsSuccess && resultRateMovie.IsSuccess)
            {
                return RedirectToAction("Details", "Movie", new { id = createReviewVM.ReviewDto.MovieId });
            }
            else
            {
                ModelState.AddModelError("Review", "Could not add review");
            }
        }

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

        var getReviewsByMovieQuery = new GetReviewsByMovieQuery(movieId, page, pageSize);

        var getMovieQuery = new GetMovieByIdQuery(movieId);

        var result = await sender.Send(getReviewsByMovieQuery);

        var totalReviewsCount = await sender.Send(totalReviewsCountQuery);

        var movieResult = await sender.Send(getMovieQuery);

        var pagedResult = new PagedResultVM<ReviewDto>
        {
            Items = result.ReviewDtos.ToList(),
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalReviewsCount.ReviewsCount
        };

        movieReviewsVM.Result = pagedResult;

        movieReviewsVM.MovieTitle = $"Reviews for {movieResult.Movie.Title}";

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
