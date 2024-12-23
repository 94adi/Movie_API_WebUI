namespace Movie.WebUI.Services.Abstractions;

public interface IReviewService
{
    Task<CreateReviewResultDto> AddReview(CreateReviewDto reviewDto);

    Task<GetAllMovieReviewsResultDto> GetMovieReviews(int movieId);

    Task GetReviewById(int reviewId);
}
