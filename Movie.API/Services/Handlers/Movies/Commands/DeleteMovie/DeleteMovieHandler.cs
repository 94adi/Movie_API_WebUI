namespace Movie.API.Services.Handlers.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand(int Id) : ICommand<DeleteMovieResult>;

public record DeleteMovieResult(bool IsSuccess, string ErrorMessage);

public class DeleteMovieHandler(IMovieRepository repository,
    IMapper mapper)
    : ICommandHandler<DeleteMovieCommand, DeleteMovieResult>
{
    public async Task<DeleteMovieResult> Handle(DeleteMovieCommand command, 
        CancellationToken cancellationToken)
    {
        try
        {
            var movie = await repository.GetByIdAsync(command.Id);

            if (movie == null)
            {
                throw new NotFoundException("Movie could not be found");
            }

            await repository.RemoveAsync(movie);

            return new DeleteMovieResult(true, null);
        }
        catch (Exception) 
        {
            throw;
        }
    }
}
