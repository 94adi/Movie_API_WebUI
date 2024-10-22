namespace Movie.API.Services.Handlers.Movies.Commands.UpdateMovie;

public record UpdateMovieCommand(
int Id,
string Title,
float Rating,
string Description,
IFormFile Image,
DateOnly ReleaseDate) : ICommand<UpdateMovieResult>;

public record UpdateMovieResult(bool IsSuccess, string ErrorMessage);

internal class UpdateMovieCommandHandler(IMovieRepository repository,
    IMapper mapper,
    IMovieService movieService) : ICommandHandler<UpdateMovieCommand, UpdateMovieResult>
{
    public async Task<UpdateMovieResult> Handle(UpdateMovieCommand command, 
        CancellationToken cancellationToken)
    {
        try
        {
            var movie = mapper.Map<Models.Movie>(command);

            await movieService.StoreMoviePoster(movie, command.Image);

            var result = await repository.UpdateAsync(movie);

            return new UpdateMovieResult(true, string.Empty);
        }
        catch(Exception ex)
        {
            return new UpdateMovieResult(false, ex.Message);
        }      
    }
}
