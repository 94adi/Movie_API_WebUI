namespace Movie.WebUI.Services.Handlers.Movies.Commands;

public record UpdateMovieCommand(UpdateMovieDto Movie) : ICommand<UpdateMovieResult>;

public record UpdateMovieResult(bool IsSuccess);

public class UpdateMovieCommandHandler(IMovieService movieService,
    IMapper mapper)
    : ICommandHandler<UpdateMovieCommand, UpdateMovieResult>
{
    public async Task<UpdateMovieResult> Handle(UpdateMovieCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await movieService.UpdateMovie(command.Movie);

        return new UpdateMovieResult(result.IsSuccess);

    }
}
