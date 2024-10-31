using Movie.API.Models.Dto;

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

        foreach (var movie in movies) 
        {
            var movieDto = movieDtos.FirstOrDefault(m => m.Id == movie.Id);
            if(movieDto != null)
            {
                var genres = movie.MovieGenres.Select(mg => new GenreDto 
                {
                    Id = mg.Genre.Id,
                    Name = mg.Genre.Name
                }).ToList();

                movieDto.Genres.AddRange(genres);
            }
        }

        return new GetMoviesResult { MovieDtos = movieDtos };
    }
}
