namespace Movie.API.Services.Review;

public interface IReviewService
{
    Task<Models.Review> GetReviewById(int reviewId);

    Task<IList<Models.Review>> GetReviewsByMovieId(int movieId, int pageSize = 0, int pageNumber = 1);

    Task<IList<Models.Review>> GetReviewsByUserId(string userId, int pageSie = 0, int pageNumber = 1);

    Task<int> GetReviewsCountByMovieId(int movieId);

    Task AddReview(Models.Review review);

    Task UpdateReview(Models.Review review);

    Task RateReview(string userId, int reviewId, ReviewRating reviewRating);

    Task DeleteReview(int reviewId);

    Task<Models.Review> GetUserMovieReview(int movieId, string userId);
}
