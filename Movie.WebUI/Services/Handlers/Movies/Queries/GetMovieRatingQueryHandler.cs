namespace Movie.WebUI.Services.Handlers.Movies.Queries;

public record GetMovieRatingQuery(int MovieId) : IQuery<GetMovieRatingResult>;

public record GetMovieRatingResult(decimal? Rating);


public class GetMovieRatingQueryHandler(IMovieService movieService)
    : IQueryHandler<GetMovieRatingQuery, GetMovieRatingResult>
{
    public async Task<GetMovieRatingResult> Handle(GetMovieRatingQuery query,
        CancellationToken cancellationToken)
    {
        var result = await movieService.GetMovieRating(query.MovieId);

        if(result == null)
        {
            return new GetMovieRatingResult(null);
        }

        return new GetMovieRatingResult(result.Rating);
    }
}
