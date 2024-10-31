namespace Movie.API.Services.Handlers.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(int Id) : ICommand<DeleteGenreResult>;

public record DeleteGenreResult(bool IsSuccess, string ErrorMessage);

internal class DeleteGenreCommandHandler(IGenreRepository repository)
    : ICommandHandler<DeleteGenreCommand, DeleteGenreResult>
{
    public async Task<DeleteGenreResult> Handle(DeleteGenreCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var genre = await repository.GetAsync(g => g.Id == command.Id);
            if (genre == null)
            {
                throw new NotFoundException("Could not find genre to delete");
            }

            await repository.RemoveAsync(genre);
        }
        catch (Exception ex)
        {
            return new DeleteGenreResult(false, ex.Message);
        }

        return new DeleteGenreResult(true, null);
    }
}
