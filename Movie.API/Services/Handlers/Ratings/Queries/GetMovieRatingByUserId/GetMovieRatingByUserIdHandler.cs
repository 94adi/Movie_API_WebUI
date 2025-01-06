using Movie.API.Services.Rating;

namespace Movie.API.Services.Handlers.Ratings.Queries.GetMovieRatingByUserId;

public record GetMovieRatingByUserIdQuery(int MovieId, string UserId) : IQuery<GetMovieRatingByUserId>;

public record GetMovieRatingByUserId(Models.Dto.RatingDto Rating);


public class GetMovieRatingByUserIdHandler(IRatingService ratingService,
    IMapper mapper) 
    : IQueryHandler<GetMovieRatingByUserIdQuery, GetMovieRatingByUserId>
{
    public async Task<GetMovieRatingByUserId> Handle(GetMovieRatingByUserIdQuery query, CancellationToken cancellationToken)
    {
        var result = await ratingService.GetMovieRatingByUser(query.MovieId, query.UserId);

        if (result == null)
        {
            new GetMovieRatingByUserId(null);
        }

        var ratingDto = mapper.Map<Models.Dto.RatingDto>(result);
        return new GetMovieRatingByUserId(ratingDto);
    }
}
