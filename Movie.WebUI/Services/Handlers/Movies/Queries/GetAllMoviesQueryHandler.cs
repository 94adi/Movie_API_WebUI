using MediatR;
using Movie.BuildingBlocks.CQRS;

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

        return new GetAllMoviesResult(result.MovieDtos);
    }
}
