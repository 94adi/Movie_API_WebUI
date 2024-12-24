namespace Movie.WebUI.Services.Handlers.Reviews.Queries;

public record GetReviewsByMovieCountQuery(int MovieId) : IQuery<GetReviewsByMovieCountResult>;

public record GetReviewsByMovieCountResult(int ReviewsCount);


public class GetReviewsByMovieCountQueryHandler(IReviewService reviewService)
    : IQueryHandler<GetReviewsByMovieCountQuery, GetReviewsByMovieCountResult>
{
    public async Task<GetReviewsByMovieCountResult> Handle(GetReviewsByMovieCountQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetMovieReviewsCount(query.MovieId);

        return new GetReviewsByMovieCountResult(result.Count);
    }
}
