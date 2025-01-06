namespace Movie.API.Services.Handlers.Movies.Commands.UpdateMovie;

public record UpdateMovieCommand(
int Id,
string Title,
string Description,
string TrailerUrl,
IFormFile Image,
DateOnly ReleaseDate) : ICommand<UpdateMovieResult>;

public record UpdateMovieResult(bool IsSuccess, string ErrorMessage);

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(m => m.Title).NotEmpty().WithMessage("Title is required");

        RuleFor(m => m.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(m => m.ReleaseDate).NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Please enter a valid release date");
    }
}

internal class UpdateMovieCommandHandler(IMovieRepository repository,
    IMovieService movieService,
    ISender sender) : ICommandHandler<UpdateMovieCommand, UpdateMovieResult>
{
    public async Task<UpdateMovieResult> Handle(UpdateMovieCommand command, 
        CancellationToken cancellationToken)
    {
        try
        {
            var movie = new Models.Movie
            {
                Id = command.Id,
                Title = command.Title,
                Description = command.Description,
                TrailerUrl = command.TrailerUrl,
                ReleaseDate = command.ReleaseDate
            };


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
