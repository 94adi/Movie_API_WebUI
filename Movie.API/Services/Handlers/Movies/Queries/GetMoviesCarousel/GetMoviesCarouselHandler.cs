
namespace Movie.API.Services.Handlers.Movies.Queries.GetMoviesCarousel;

public record GetMoviesCarouselQuery() : IQuery<GetMoviesCarouselResult>;

public record GetMoviesCarouselResult(IEnumerable<Models.Movie> MovieCarousels);

public class GetMoviesCarouselQuerylHandler(IMovieCarouselRepository carouselRepo,
    IMovieRepository movieRepo,
    IMovieService movieService)
    : IQueryHandler<GetMoviesCarouselQuery, GetMoviesCarouselResult>
{
    public async Task<GetMoviesCarouselResult> Handle(GetMoviesCarouselQuery request, CancellationToken cancellationToken)
    {
        var result = await carouselRepo.GetAllAsync();

        var movieCarouselIds = result.Select(i => i.MovieId).ToList();

        var movieCarousels = await movieRepo.GetAllAsync(m => movieCarouselIds.Contains(m.Id));

        await movieService.AddPosterUrls(movieCarousels);

        return new GetMoviesCarouselResult(movieCarousels);
    }
}
