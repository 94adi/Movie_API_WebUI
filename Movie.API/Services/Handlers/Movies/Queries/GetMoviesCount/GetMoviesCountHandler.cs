
namespace Movie.API.Services.Handlers.Movies.Queries.GetMoviesCount;

public record GetMoviesCountQuery() : IQuery<GetMoviesCountResult>;

public record GetMoviesCountResult(int Count);


public class GetMoviesCountHandler(IMovieService movieService)
    : IQueryHandler<GetMoviesCountQuery, GetMoviesCountResult>
{
    public async Task<GetMoviesCountResult> Handle(GetMoviesCountQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await movieService.GetMoviesCount();

        return new GetMoviesCountResult(result);
    }
}
