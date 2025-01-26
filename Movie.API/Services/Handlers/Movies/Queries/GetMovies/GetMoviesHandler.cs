namespace Movie.API.Services.Handlers.Movies.Queries.GetMovies;

public record GetMoviesQuery(int PageNumber = 1, int PageSize = 0) : IQuery<GetMoviesResult>;

public record GetMoviesResult
{
    public IEnumerable<Models.Dto.MovieDto> MovieDtos { get; set; } = new List<Models.Dto.MovieDto>();
    public int TotalPages { get; set; }
}

internal class GetMoviesQueryHandler(IMovieService movieService,
    IMapper mapper) :
    IQueryHandler<GetMoviesQuery, GetMoviesResult>
{
    public async Task<GetMoviesResult> Handle(GetMoviesQuery query, CancellationToken cancellationToken)
    {
        var movies = await movieService.GetMoviesWithGenres(pageSize: query.PageSize,
            pageNumber: query.PageNumber);

        await movieService.AddPosterUrls(movies);

        var movieDtos = mapper.Map<List<Models.Dto.MovieDto>>(movies);

        return new GetMoviesResult { MovieDtos = movieDtos };
    }
}
