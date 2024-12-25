namespace Movie.WebUI.Services.Handlers.Movies.Queries;

public record GetMoviesCountQuery() : IQuery<GetMoviesCountQueryResult>;

public record GetMoviesCountQueryResult(int MoviesCount);

public class GetMoviesCountQueryHandler(IMovieService movieService)
    : IQueryHandler<GetMoviesCountQuery, GetMoviesCountQueryResult>
{
    public async Task<GetMoviesCountQueryResult> Handle(GetMoviesCountQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await movieService.GetMoviesCount();

        return new GetMoviesCountQueryResult(result.Count);
    }
}
