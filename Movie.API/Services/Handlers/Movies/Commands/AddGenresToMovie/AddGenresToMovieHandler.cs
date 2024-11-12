namespace Movie.API.Services.Handlers.Movies.Commands.AddGenresToMovie;

public record AddGenresToMovieCommand(List<int> GenreIds, int MovieId, bool IsUpdate = false) 
    : ICommand<AddGenresToMovieResult>;

public record AddGenresToMovieResult(bool IsSucces);

internal class AddGenresToMovieCommandHandler(IMovieService movieService)
    : ICommandHandler<AddGenresToMovieCommand, AddGenresToMovieResult>
{
    public async Task<AddGenresToMovieResult> Handle(AddGenresToMovieCommand command, 
        CancellationToken cancellationToken)
    {
        List<Models.MovieGenre> movieGenres = new();
        try
        {
            if (command.IsUpdate)
            {
                await movieService.RemoveMovieGenres(command.MovieId);
            }

            await movieService.AddMovieGenres(command.MovieId, command.GenreIds);

        }
        catch (Exception ex) 
        {
            throw;
        }

        return new AddGenresToMovieResult(true);
    }
}
