namespace Movie.API.Services.Handlers.Reviews.Queries.GetReviewById;

public record GetReviewQuery(int id) : IQuery<GetReviewResult>;

public record GetReviewResult(Models.Dto.ReviewDto Review);


internal class GetReviewQueryHandler(IReviewService reviewService,
    IMapper mapper)
    : IQueryHandler<GetReviewQuery, GetReviewResult>
{
    public async Task<GetReviewResult> Handle(GetReviewQuery query,
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetReviewById(query.id);

        var resultDto = mapper.Map<Models.Dto.ReviewDto>(result);

        return new GetReviewResult(resultDto);
    }
}
