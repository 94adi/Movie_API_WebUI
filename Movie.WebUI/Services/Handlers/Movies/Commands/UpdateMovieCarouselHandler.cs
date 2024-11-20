namespace Movie.WebUI.Services.Handlers.Movies.Commands;

public record UpdateMovieCarouselCommand(IEnumerable<string> MovieIds) : ICommand<UpdateMovieCarouselResult>;

public record UpdateMovieCarouselResult(bool IsSuccess);

internal class UpdateMovieCarouselCommandHandler(IMovieService movieService,
    IMapper mapper)
    : ICommandHandler<UpdateMovieCarouselCommand, UpdateMovieCarouselResult>
{
    public async Task<UpdateMovieCarouselResult> Handle(UpdateMovieCarouselCommand command, 
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateMovieCarouselDto>(command);

        var result = await movieService.UpdateMovieCarousel(request);

        return new UpdateMovieCarouselResult(result.IsSuccess);
    }
}
