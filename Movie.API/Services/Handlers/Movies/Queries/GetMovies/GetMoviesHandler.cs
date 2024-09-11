using Movie.API.Repository.Abstractions;
using Movie.BuildingBlocks.CQRS;

namespace Movie.API.Services.Handlers.Movies.Queries.GetMovies;

public record GetMoviesQuery(int PageNumber = 1, int PageSize = 0) : IQuery<GetMoviesResult> { }

public record GetMoviesResult
{
    public IEnumerable<Models.Movie> Movies { get; set; } = new List<Models.Movie>();
}

internal class GetMoviesQueryHandler(IMovieRepository movieRepository) :
    IQueryHandler<GetMoviesQuery, GetMoviesResult>
{
    public async Task<GetMoviesResult> Handle(GetMoviesQuery query, CancellationToken cancellationToken)
    {
        var movies = await movieRepository.GetAllAsync(pageSize: query.PageSize,
            pageNumber: query.PageNumber);

        return new GetMoviesResult { Movies = movies };
    }
}
