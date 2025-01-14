namespace Movie.WebUI.Services.Handlers.Reviews.Queries;

public record GetUserMovieReviewQuery(int MovieId, string UserId) 
    : IQuery<GetUserMovieReviewResult>;

public record GetUserMovieReviewResult(ReviewDto ReviewDto);

public class GetUserMovieReviewQueryHandler(IReviewService reviewService)
    : IQueryHandler<GetUserMovieReviewQuery, GetUserMovieReviewResult>
{
    public async Task<GetUserMovieReviewResult> Handle(GetUserMovieReviewQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetUserMovieReview(query.MovieId, query.UserId);

        return new GetUserMovieReviewResult(result.Review);
    }
}
