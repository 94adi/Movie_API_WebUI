using System.Linq.Expressions;

namespace Movie.API.Repository;

public class ReviewRepository : Repository<Models.Review>, IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Review> GetReviewById(int reviewId)
    {
        return await _context.Reviews
            .Where(r => r.Id == reviewId)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<Review>> GetReviewsByMovieId(int movieId, int pageSize = 0, int pageNumber = 1)
    {
        Expression<Func<Review, bool>> filter = (review => review.MovieId == movieId);

        var result = await GetAllAsync(filter: filter,
            pageSize: pageSize,
            pageNumber: pageNumber);

        return result;
    }

    public async Task<IList<Review>> GetReviewsByUserId(string userId, int pageSize = 0, int pageNumber = 1)
    {
        Expression<Func<Review, bool>> filter = (review => review.UserId == userId);

        var result = await GetAllAsync(filter: filter,
            pageSize: pageSize,
            pageNumber: pageNumber);

        return result;
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Attach(review);
        _context.Entry(review).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
