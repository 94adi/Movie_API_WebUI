namespace Movie.WebUI.Services.Handlers.Ratings.Queries;

public record GetMovieRatingByUserQuery(int MovieId) : IQuery<GetMovieRatingByUserResult>;

public record GetMovieRatingByUserResult(RatingDto Rating);

public class GetMovieRatingByUserQueryHandler(IReviewService reviewService)
    : IQueryHandler<GetMovieRatingByUserQuery, GetMovieRatingByUserResult>
{
    public async Task<GetMovieRatingByUserResult> Handle(GetMovieRatingByUserQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetMovieRatingByUser(query.MovieId);
        if(result == null)
        {
            return new GetMovieRatingByUserResult(null);
        }
        return new GetMovieRatingByUserResult(result.Rating);
    }
}
