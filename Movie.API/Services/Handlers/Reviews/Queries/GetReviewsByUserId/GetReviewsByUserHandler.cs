
namespace Movie.API.Services.Handlers.Reviews.Queries.GetReviewsByUserId;

public record GetReviewsByUserQuery(string UserId,
    int PageNumber = 1,
    int PageSize = 0) : IQuery<GetReviewsByUserResult>;

public record GetReviewsByUserResult(IList<Models.Dto.ReviewDto> Reviews);

internal class GetReviewsByUserQueryHandler(IReviewService reviewService,
    IMapper mapper)
    : IQueryHandler<GetReviewsByUserQuery, GetReviewsByUserResult>
{
    public async Task<GetReviewsByUserResult> Handle(GetReviewsByUserQuery query, 
        CancellationToken cancellationToken)
    {
        var reviews = await reviewService.GetReviewsByUserId(query.UserId, 
            query.PageNumber, 
            query.PageSize);

        var reviewsDto = mapper.Map<IList<Models.Dto.ReviewDto>>(reviews);

        return new GetReviewsByUserResult(reviewsDto);
    }
}
