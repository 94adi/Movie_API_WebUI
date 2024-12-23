namespace Movie.API.Services.Handlers.Reviews.Queries.GetReviewsByMovie;

public record GetReviewsByMovieQuery(int MovieId, 
    int PageNumber = 1, 
    int PageSize = 0) : IQuery<GetReviewsByMovieResult>;

public record GetReviewsByMovieResult(IList<Models.Dto.ReviewDto> ReviewDtos);

internal class GetReviewsByMovieQueryHandler(IReviewService reviewService,
    IMapper mapper)
    : IQueryHandler<GetReviewsByMovieQuery, GetReviewsByMovieResult>
{
    public async Task<GetReviewsByMovieResult> Handle(GetReviewsByMovieQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await reviewService.GetReviewsByMovieId(query.MovieId, 
            query.PageSize, 
            query.PageNumber);

        var resultDto = mapper.Map<IList<Models.Dto.ReviewDto>>(result);

        return new GetReviewsByMovieResult(resultDto);
    }
}
