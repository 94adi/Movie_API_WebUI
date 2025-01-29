namespace Movie.API.Services.Handlers.Genres.Commands.UpdateGenre;

public record UpdateGenreCommand(int Id, string Name) : ICommand<UpdateGenreResult>;

public record UpdateGenreResult(bool IsSuccess, string ErrorMessage);

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(g => g.Name).NotEmpty().WithMessage("Genre name is required");
    }
}

internal class UpdateGenreHandler(IGenreRepository repository,
    IMapper mapper) :
    ICommandHandler<UpdateGenreCommand, UpdateGenreResult>
{
    public async Task<UpdateGenreResult> Handle(UpdateGenreCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var genre = mapper.Map<Genre>(command);

            await repository.UpdateAsync(genre);
        }
        catch(Exception ex)
        {
            return new UpdateGenreResult(false, ex.Message);
        }

        return new UpdateGenreResult(true, null);
    }
}
