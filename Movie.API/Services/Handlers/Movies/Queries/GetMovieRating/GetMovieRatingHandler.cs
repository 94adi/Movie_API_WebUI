using Movie.API.Services.Rating;

namespace Movie.API.Services.Handlers.Movies.Queries.GetMovieRating;

public record GetMovieRatingQuery(int MovieId): IQuery<GetMovieRatingResult>;

public record GetMovieRatingResult(decimal Rating);

public class GetMovieRatingQueryHandler(IRatingService ratingService) :
    IQueryHandler<GetMovieRatingQuery, GetMovieRatingResult>
{
    public async Task<GetMovieRatingResult> Handle(GetMovieRatingQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await ratingService.GetMovieFinalRating(query.MovieId);

        return new GetMovieRatingResult(result);
    }
}
