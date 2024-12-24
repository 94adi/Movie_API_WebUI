namespace Movie.API.Services.Review;

public class ReviewService(IReviewRepository reviewRepo) : IReviewService
{
    public async Task<Models.Review> GetReviewById(int reviewId)
    {
        return await reviewRepo.GetReviewById(reviewId);
    }

    public async Task<IList<Models.Review>> GetReviewsByMovieId(int movieId, int pageSize = 0, int pageNumber = 1)
    {
        return await reviewRepo.GetReviewsByMovieId(movieId, pageSize, pageNumber);
    }

    public async Task<IList<Models.Review>> GetReviewsByUserId(string userId, int pageSie = 0, int pageNumber = 1)
    {
        return await reviewRepo.GetReviewsByUserId(userId);
    }

    public async Task AddReview(Models.Review review)
    {
        review.CreatedAt = DateTime.Now;
        await reviewRepo.CreateAsync(review);
    }

    public async Task RateReview(string userId, int reviewId, ReviewRating reviewRating)
    {
        var review = await GetReviewById(reviewId);

        //
        //if(String.Compare(review.UserId, userId) != 0)
        //{
        //    throw new ArgumentException("User id does not match review user id");
        //}

        if (reviewRating == ReviewRating.THUMBS_UP) 
        {
            review.NoAgree++;
        }
        else
        {
            review.NoDisagree++;
        }

        await reviewRepo.UpdateAsync(review);

    }

    public async Task DeleteReview(int reviewId)
    {
        try
        {
            var review = await GetReviewById(reviewId);

            if (review == null)
            {
                throw new NotFoundException("Movie could not be found");
            }

            await reviewRepo.RemoveAsync(review);
        }
        catch (Exception) 
        {
            throw;
        }

    }

    public async Task<int> GetReviewsCountByMovieId(int movieId)
    {
        return await reviewRepo.GetReviewsCountByMovieId(movieId);
    }
}
