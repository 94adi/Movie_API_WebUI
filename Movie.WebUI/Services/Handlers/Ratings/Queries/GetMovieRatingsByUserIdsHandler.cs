
namespace Movie.WebUI.Services.Handlers.Ratings.Queries;

public record GetMovieRatingsByUserIdsQuery(int MovieId, IEnumerable<string> UserIds) 
    : IQuery<GetMovieRatingsByUserIdsResult>;

public record GetMovieRatingsByUserIdsResult(IEnumerable<Models.Dto.RatingDto> RatingDtos);

public class GetMovieRatingsByUserIdsQueryHandler(IMovieService movieService)
    : IQueryHandler<GetMovieRatingsByUserIdsQuery, GetMovieRatingsByUserIdsResult>
{
    public async Task<GetMovieRatingsByUserIdsResult> Handle(GetMovieRatingsByUserIdsQuery query, 
        CancellationToken cancellationToken)
    {
        var request = new GetMovieRatingsRequestDto(query.UserIds);
        var result = await movieService.GetMovieRatings(query.MovieId, request);

        return new GetMovieRatingsByUserIdsResult(result.Ratings);
    }
}
