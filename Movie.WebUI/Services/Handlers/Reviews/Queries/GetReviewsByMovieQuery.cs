namespace Movie.WebUI.Services.Handlers.Reviews.Queries;

public record GetReviewsByMovieQuery(int movieId) : IQuery<GetReviewsByMovieResult>;

public record GetReviewsByMovieResult(IList<ReviewDto> ReviewDtos);

public class GetReviewsByMovieQueryHandler(IReviewService reviewService) :
    IQueryHandler<GetReviewsByMovieQuery, GetReviewsByMovieResult>
{
    public async Task<GetReviewsByMovieResult> Handle(GetReviewsByMovieQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetMovieReviews(query.movieId);

        return new GetReviewsByMovieResult(result.ReviewDtos);
    }
}
