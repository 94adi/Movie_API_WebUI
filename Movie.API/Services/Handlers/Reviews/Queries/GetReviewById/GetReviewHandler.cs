namespace Movie.API.Services.Handlers.Reviews.Queries.GetReviewById;

public record GetReviewQuery(int id) : IQuery<GetReviewResult>;

public record GetReviewResult(Models.Review Review);


internal class GetReviewQueryHandler(IReviewService reviewService)
    : IQueryHandler<GetReviewQuery, GetReviewResult>
{
    public async Task<GetReviewResult> Handle(GetReviewQuery query,
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetReviewById(query.id);

        return new GetReviewResult(result);
    }
}
