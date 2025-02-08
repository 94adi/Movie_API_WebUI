
namespace Movie.WebUI.Services.Handlers.Movies.Queries;

public record GetMoviesCarouselQuery() : IQuery<GetMoviesCarouselResult>;

public record GetMoviesCarouselResult(IEnumerable<Models.Dto.MovieDto> MovieCarousels);


public class GetMoviesCarouselQueryHandler(IMovieService movieService)
    : IQueryHandler<GetMoviesCarouselQuery, GetMoviesCarouselResult>
{
    public async Task<GetMoviesCarouselResult> Handle(GetMoviesCarouselQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await movieService.GetAllMoviesCarousel();
        if(result != null && result.MovieCarousels != null && result.MovieCarousels.Any())
        {
            return new GetMoviesCarouselResult(result.MovieCarousels);
        }

        return new GetMoviesCarouselResult(new List<MovieDto>());
    }
}
