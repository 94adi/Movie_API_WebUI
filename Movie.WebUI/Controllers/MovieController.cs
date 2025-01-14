using Movie.WebUI.Utils;

namespace Movie.WebUI.Controllers;

public class MovieController(ISender sender,
    ITokenProvider tokenProvider,
    IMapper mapper) : Controller
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
    public async Task<IActionResult> AddEditReview(int id)
    {
        UpsertReviewVM upsertReviewVM = new UpsertReviewVM();

        var userId = tokenProvider.GetUserId();

        upsertReviewVM.ReviewDto.UserId = userId;
        upsertReviewVM.ReviewDto.MovieId = id;
        upsertReviewVM.RatingDto.UserId = userId;
        upsertReviewVM.RatingDto.MovieId = id;

        var getMovieQuery = new GetMovieByIdQuery(id);
        var movieResult = await sender.Send(getMovieQuery);

        //if movieResult is null redirect to error page
        
        var reviewQuery = new GetUserMovieReviewQuery(movieResult.Movie.Id, userId);

        var reviewResult = await sender.Send(reviewQuery);

        var review = reviewResult.ReviewDto;

        if (review != null)
        {
            PopulateReviewValues(upsertReviewVM, review);
            var userMovieRatingQuery = new GetMovieRatingByUserQuery(movieResult.Movie.Id);
            var userMovieRating = await sender.Send(userMovieRatingQuery);
            upsertReviewVM.RatingDto.RatingValue = userMovieRating.Rating.RatingValue;
            upsertReviewVM.PageTitle = $"Edit review for {movieResult.Movie.Title}";
        }
        else
        {
            upsertReviewVM.PageTitle = $"Add review for: {movieResult.Movie.Title}";
        }
        return View(upsertReviewVM);
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddEditReview([FromForm] UpsertReviewVM viewModel)
    {
        if (ModelState.IsValid)
        {
            bool isReviewSubmitted = false;

            var rateMovieCommand = new RateMovieCommand(viewModel.RatingDto.MovieId,
                viewModel.RatingDto.RatingValue);

            var resultRateMovie = await sender.Send(rateMovieCommand);

            if (viewModel.ReviewDto.Id.HasValue)
            {
                var updateReviewCommand = mapper.Map<UpdateReviewCommand>(viewModel.ReviewDto);
                var updateReviewResult = await sender.Send(updateReviewCommand);
                isReviewSubmitted = updateReviewResult.IsSuccess;
            }
            else
            {
                var addReviewCommand = new AddReviewCommand(viewModel.ReviewDto);
                var resultAddReview = await sender.Send(addReviewCommand);
                isReviewSubmitted = resultAddReview.IsSuccess;
            }


            if (isReviewSubmitted && resultRateMovie.IsSuccess)
            {
                return RedirectToAction("Details", "Movie", new { id = viewModel.ReviewDto.MovieId });
            }
            else
            {
                ModelState.AddModelError("Review", "Could not submit review");
            }
        }

        return View(viewModel);
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

        var reviewUserIds = result.ReviewDtos.Select(r => r.UserId).Distinct().ToList();

        var getMovieRatingsByUserIdsQuery = new GetMovieRatingsByUserIdsQuery(movieId, reviewUserIds);

        var movieRatingsByUserIds = await sender.Send(getMovieRatingsByUserIdsQuery);

        var totalReviewsCount = await sender.Send(totalReviewsCountQuery);

        var movieResult = await sender.Send(getMovieQuery);

        foreach (var review in result.ReviewDtos)
        {
            var ratingValue = movieRatingsByUserIds.RatingDtos
                    .FirstOrDefault(r => r.UserId == review.UserId)?.RatingValue;
            if (ratingValue.HasValue)
            {
                review.Rating = ratingValue.Value;
            }
        } 


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

    private void PopulateReviewValues(UpsertReviewVM viewModel, ReviewDto review)
    {
        viewModel.ReviewDto.Id = review.Id;
        viewModel.ReviewDto.Title = review.Title;
        viewModel.ReviewDto.Content = review.Content;
        viewModel.RatingDto.RatingValue = review.Rating;
    }
}
