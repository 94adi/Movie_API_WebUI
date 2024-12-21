
namespace Movie.API.Services.Handlers.Movies.Queries.GetMoviesCarousel;

public record GetMoviesCarouselQuery() : IQuery<GetMoviesCarouselResult>;

public record GetMoviesCarouselResult(IEnumerable<Models.Movie> MovieCarousels);

public class GetMoviesCarouselQuerylHandler(IMovieCarouselRepository carouselRepo,
    IMovieRepository movieRepo)
    : IQueryHandler<GetMoviesCarouselQuery, GetMoviesCarouselResult>
{
    public async Task<GetMoviesCarouselResult> Handle(GetMoviesCarouselQuery request, CancellationToken cancellationToken)
    {
        var result = await carouselRepo.GetAllAsync();

        var movieCarouselIds = result.Select(i => i.MovieId).ToList();

        var movieCarousels = await movieRepo.GetAllAsync(m => movieCarouselIds.Contains(m.Id));

        return new GetMoviesCarouselResult(movieCarousels);
    }
}
