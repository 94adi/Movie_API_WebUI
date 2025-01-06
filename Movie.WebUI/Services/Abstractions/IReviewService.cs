namespace Movie.WebUI.Services.Abstractions;

public interface IReviewService
{
    Task<CreateReviewResultDto> AddReview(CreateReviewDto reviewDto);

    Task<GetAllMovieReviewsResultDto> GetMovieReviews(int movieId, int pageNumber, int pageSize);

    Task GetReviewById(int reviewId);

    Task<GetMovieReviewsCountResultDto> GetMovieReviewsCount(int movieId);

    Task<GetMovieRatingByUserResultDto> GetMovieRatingByUser(int movieId);
}
