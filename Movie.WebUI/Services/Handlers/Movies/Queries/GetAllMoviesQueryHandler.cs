namespace Movie.WebUI.Services.Handlers.Movies.Queries;

public record GetAllMoviesQuery() : IQuery<GetAllMoviesResult>;

public record GetAllMoviesResult(IEnumerable<MovieDto> MovieDtos);


public class GetAllMoviesQueryHandler(IMovieService movieService)
    : IQueryHandler<GetAllMoviesQuery, GetAllMoviesResult>
{
    public async Task<GetAllMoviesResult> Handle(GetAllMoviesQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await movieService.GetAllMovies();

        if(result == null || result.MovieDtos == null || (!result.MovieDtos.Any()))
        {
            return new GetAllMoviesResult(new List<MovieDto>());
        }
        return new GetAllMoviesResult(result.MovieDtos);      
    }
}
