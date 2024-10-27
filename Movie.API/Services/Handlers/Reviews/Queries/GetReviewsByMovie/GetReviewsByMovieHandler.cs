namespace Movie.API.Services.Handlers.Reviews.Queries.GetReviewsByMovie;

public record GetReviewsByMovieQuery(int MovieId, 
    int PageNumber = 1, 
    int PageSize = 0) : IQuery<GetReviewsByMovieResult>;

public record GetReviewsByMovieResult(IList<Models.Review> Reviews);

internal class GetReviewsByMovieQueryHandler(IReviewService reviewService)
    : IQueryHandler<GetReviewsByMovieQuery, GetReviewsByMovieResult>
{
    public async Task<GetReviewsByMovieResult> Handle(GetReviewsByMovieQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetReviewsByMovieId(query.MovieId, 
            query.PageSize, 
            query.PageNumber);

        return new GetReviewsByMovieResult(result);
    }
}
