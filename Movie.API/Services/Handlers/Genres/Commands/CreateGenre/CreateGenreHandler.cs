namespace Movie.API.Services.Handlers.Genres.Commands.CreateGenre;

public record CreateGenreCommand(string Name) : ICommand<CreateGenreResult>;

public record CreateGenreResult(int Id);

internal class CreateGenreCommandHandler(IGenreRepository repository) 
    : ICommandHandler<CreateGenreCommand, CreateGenreResult>
{
    public async Task<CreateGenreResult> Handle(CreateGenreCommand command,
        CancellationToken cancellationToken)
    {
        var genre = new Genre
        {
            Name = command.Name
        };

        await repository.CreateAsync(genre);

        return new CreateGenreResult(genre.Id);
    }
}
