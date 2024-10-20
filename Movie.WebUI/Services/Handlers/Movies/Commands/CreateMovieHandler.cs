namespace Movie.WebUI.Services.Handlers.Movies.Commands;

public record CreateMovieCommand(CreateMovieDto Movie) : ICommand<CreateMovieResult>;

public record CreateMovieResult(int Id, bool IsSuccess);

public class CreateMovieCommandHandler(IMovieService movieService)
    : ICommandHandler<CreateMovieCommand, CreateMovieResult>
{
    public async Task<CreateMovieResult> Handle(CreateMovieCommand command, 
        CancellationToken cancellationToken)
    {
        var movie = command.Movie;

        var response = await movieService.CreateMovie(movie);

        if (response != null)
        {
            return new CreateMovieResult(response.Id, true);
        }

        return new CreateMovieResult(int.MinValue, false);
    }
}
