namespace Movie.API.Services.Handlers.Movies.Queries.GetMovies;

public record GetMoviesQuery(int PageNumber = 1, int PageSize = 0) : IQuery<GetMoviesResult>;

public record GetMoviesResult
{
    public IEnumerable<Models.Dto.MovieDto> MovieDtos { get; set; } = new List<Models.Dto.MovieDto>();
}

internal class GetMoviesQueryHandler(IMovieRepository movieRepository,
    IMapper mapper) :
    IQueryHandler<GetMoviesQuery, GetMoviesResult>
{
    public async Task<GetMoviesResult> Handle(GetMoviesQuery query, CancellationToken cancellationToken)
    {
        var movies = await movieRepository.GetMoviesWithGenres(pageSize: query.PageSize,
            pageNumber: query.PageNumber);

        var movieDtos = mapper.Map<List<Models.Dto.MovieDto>>(movies);

        return new GetMoviesResult { MovieDtos = movieDtos };
    }
}
