namespace Movie.WebUI.Services.Handlers.Reviews.Queries;

public record GetReviewByIdQuery(int Id) : IQuery<GetReviewByIdResult>;

public record GetReviewByIdResult(ReviewDto ReviewDto);


public class GetReviewByIdQueryHandler(IReviewService reviewService) 
    : IQueryHandler<GetReviewByIdQuery, GetReviewByIdResult>
{
    public async Task<GetReviewByIdResult> Handle(GetReviewByIdQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetReviewById(query.Id);

        return new GetReviewByIdResult(result.ReviewDto);
    }
}
