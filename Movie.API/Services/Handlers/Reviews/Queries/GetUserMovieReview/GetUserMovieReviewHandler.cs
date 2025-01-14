namespace Movie.API.Services.Handlers.Reviews.Queries.GetUserMovieReview;

public record GetUserMovieReviewQuery(int MovieId, string UserId) : IQuery<GetUserMovieReviewResult>;

public record GetUserMovieReviewResult(Models.Review Review);

internal class GetUserMovieReviewQueryHandler(IReviewService reviewService)
    : IQueryHandler<GetUserMovieReviewQuery, GetUserMovieReviewResult>
{
    public async Task<GetUserMovieReviewResult> Handle(GetUserMovieReviewQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetUserMovieReview(query.MovieId, query.UserId);

        return new GetUserMovieReviewResult(result);
    }
}
