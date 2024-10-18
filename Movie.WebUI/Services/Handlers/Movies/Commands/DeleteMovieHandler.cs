
namespace Movie.WebUI.Services.Handlers.Movies.Commands;

public record DeleteMovieCommand(int Id) : ICommand<DeleteMovieResult>;

public record DeleteMovieResult(bool IsSuccess);

internal class DeleteMovieHandler(IMovieService movieService) 
    : ICommandHandler<DeleteMovieCommand, DeleteMovieResult>
{
    public async Task<DeleteMovieResult> Handle(DeleteMovieCommand command, 
        CancellationToken cancellationToken)
    {
        var movieId = command.Id;
        var result = await movieService.DeleteMovie(movieId);

        return new DeleteMovieResult(result.IsSuccess);
    }
}
