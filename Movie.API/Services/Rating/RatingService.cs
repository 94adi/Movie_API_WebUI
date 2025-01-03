﻿
namespace Movie.API.Services.Rating;

public class RatingService(IRatingRepository ratingRepo) : IRatingService
{
    public async Task AddRating(Models.Rating rating)
    {
        await ratingRepo.CreateAsync(rating);
    }

    public async Task<double> GetMovieFinalRating(int movieId)
    {
        double finalRating = 0.0;

        var ratings = await this.GetMovieRatings(movieId);

        if (ratings != null) 
        {
            foreach (var rating in ratings) 
            {
                finalRating += rating.RatingValue;
            }
            finalRating /= ratings.Count();
        }

        return finalRating;
    }

    public async Task<Models.Rating> GetMovieRatingByUser(int movieId, string userId)
    {
        var rating = await ratingRepo.GetAsync(r => r.UserId == userId && 
                            r.MovieId == movieId);

        return rating;
    }

    public async Task<IList<Models.Rating>> GetMovieRatings(int movieId)
    {
        var ratings = await ratingRepo.GetAllAsync(r => r.MovieId == movieId);

        return ratings;
    }

    public async Task UpdateRating(Models.Rating rating)
    {
        await ratingRepo.UpdateAsync(rating);
    }
}
