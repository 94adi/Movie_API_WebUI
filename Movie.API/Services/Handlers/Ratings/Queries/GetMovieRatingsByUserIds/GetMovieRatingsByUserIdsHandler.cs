
namespace Movie.API.Services.Handlers.Ratings.Queries.GetMovieRatingsByUserIds;

public record GetMovieRatingsByUserIdsQuery(int MovieId, IEnumerable<string> UserIds) 
    : IQuery<GetMovieRatingsByUserIdsResult>;

public record GetMovieRatingsByUserIdsResult(IEnumerable<Models.Dto.RatingDto> Ratings);

public class GetMovieRatingsByUserIdsQueryHandler(IRatingService ratingService,
    IMapper mapper)
    : IQueryHandler<GetMovieRatingsByUserIdsQuery, GetMovieRatingsByUserIdsResult>
{
    public async Task<GetMovieRatingsByUserIdsResult> Handle(GetMovieRatingsByUserIdsQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await ratingService.GetMovieRatingsByUsers(query.MovieId, query.UserIds);

        if(result == null)
        {
            return new GetMovieRatingsByUserIdsResult(null);
        }

        var ratingDtos = mapper.Map<IEnumerable<Models.Dto.RatingDto>>(result);

        return new GetMovieRatingsByUserIdsResult(ratingDtos);
    }
}
