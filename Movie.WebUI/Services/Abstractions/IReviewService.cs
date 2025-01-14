namespace Movie.WebUI.Services.Abstractions;

public interface IReviewService
{
    Task<CreateReviewResultDto> AddReview(UpsertReviewDto reviewDto);

    Task<GetAllMovieReviewsResultDto> GetMovieReviews(int movieId, int pageNumber, int pageSize);

    Task<GetReviewByIdResultDto> GetReviewById(int reviewId);

    Task<GetMovieReviewsCountResultDto> GetMovieReviewsCount(int movieId);

    Task<GetMovieRatingByUserResultDto> GetMovieRatingByUser(int movieId);

    Task<UpdateReviewResultDto> UpdateReview(UpdateReviewDto updateReviewDto);

    Task<GetUserMovieReviewResultDto> GetUserMovieReview(int movieId, string userId);
}
