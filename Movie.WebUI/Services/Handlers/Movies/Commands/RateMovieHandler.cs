namespace Movie.WebUI.Services.Handlers.Movies.Commands;

public record RateMovieCommand(int MovieId, int RatingValue) : ICommand<RateMovieCommandResult>;

public record RateMovieCommandResult(bool IsSuccess);

public class RateMovieCommandHandler(IMapper mapper,
    IMovieService movieService) : ICommandHandler<RateMovieCommand, RateMovieCommandResult>
{
    public async Task<RateMovieCommandResult> Handle(RateMovieCommand command, CancellationToken cancellationToken)
    {
        var request = mapper.Map<RateMovieDto>(command);

        var result = await movieService.RateMovie(request);

        return new RateMovieCommandResult(result.IsSuccess);
    }
}
