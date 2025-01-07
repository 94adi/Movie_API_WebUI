namespace Movie.API.Services.Rating;

public interface IRatingService
{
    Task AddRating(Models.Rating rating);

    Task UpdateRating(Models.Rating rating);

    Task<Models.Rating> GetMovieRatingByUser(int movieId, string userId);

    Task<IList<Models.Rating>> GetMovieRatings(int movieId);

    Task<decimal> GetMovieFinalRating(int movieId);
}
