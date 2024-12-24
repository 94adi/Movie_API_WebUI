namespace Movie.API.Repository.Abstractions;

public interface IReviewRepository : IRepository<Models.Review>
{
    Task<Models.Review> GetReviewById(int reviewId);

    Task<IList<Models.Review>> GetReviewsByMovieId(int movieId, int pageSize = 0, int pageNumber = 1);

    Task<IList<Models.Review>> GetReviewsByUserId(string userId, int pageSize = 0, int pageNumber = 1);

    Task UpdateAsync(Models.Review review);

    Task<int> GetReviewsCountByMovieId(int movieId);
}
