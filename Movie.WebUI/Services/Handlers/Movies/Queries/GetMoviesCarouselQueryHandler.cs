
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

        return new GetMoviesCarouselResult(result.MovieCarousels);
    }
}
