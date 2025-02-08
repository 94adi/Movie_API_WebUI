
namespace Movie.WebUI.Services.Handlers.Movies.Queries
{
    public record GetMoviesByPagingQuery(int PageNumber, int PageSize) 
        : IQuery<GetMoviesByPagingResult>;

    public record GetMoviesByPagingResult(IEnumerable<MovieDto> MovieDtos);


    public class GetMoviesByPagingQueryHandler(IMovieService movieService)
        : IQueryHandler<GetMoviesByPagingQuery, GetMoviesByPagingResult>
    {
        public async Task<GetMoviesByPagingResult> Handle(GetMoviesByPagingQuery query,
            CancellationToken cancellationToken)
        {
            var result = await movieService.GetMovies(query.PageNumber, query.PageSize);

            if(result != null && result.MovieDtos !=null && result.MovieDtos.Any())
            {
                return new GetMoviesByPagingResult(result.MovieDtos);
            }

            return new GetMoviesByPagingResult(new List<MovieDto>());
        }
    }
}
