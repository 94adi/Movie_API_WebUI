namespace Movie.API.Services.Handlers.Reviews.Queries.GetReviewsCountByMovie;

public record GetReviewsCountByMovieQuery(int MovieId) : IQuery<GetReviewsCountByMovieResult>;

public record GetReviewsCountByMovieResult(int Count);

internal class GetReviewsCountByMovieQueryHandler(IReviewService reviewService)
    : IQueryHandler<GetReviewsCountByMovieQuery, GetReviewsCountByMovieResult>
{
    public async Task<GetReviewsCountByMovieResult> Handle(GetReviewsCountByMovieQuery query, 
        CancellationToken cancellationToken)
    {
        var result =  await reviewService.GetReviewsCountByMovieId(query.MovieId);

        return new GetReviewsCountByMovieResult(result);
    }
}
