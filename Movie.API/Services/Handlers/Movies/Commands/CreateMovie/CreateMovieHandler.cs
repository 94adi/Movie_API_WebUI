namespace Movie.API.Services.Handlers.Movies.CreateMovie;

public record CreateMovieCommand(string Title, 
    string Description,
    IFormFile Image,
    string TrailerUrl,
DateOnly ReleaseDate) : ICommand<CreateMovieResult>;

public record CreateMovieResult(int Id);

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(m => m.Title)
            .NotEmpty()
            .WithMessage("Title is required");

        RuleFor(m => m.Description)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(m => m.ReleaseDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Please enter a valid release date");
    }
}


internal class CreateMovieCommandHandler(IMovieRepository repository, 
    IMovieService movieService,
    ISender sender) : 
    ICommandHandler<CreateMovieCommand, CreateMovieResult>
{
    public async Task<CreateMovieResult> Handle(CreateMovieCommand command, 
        CancellationToken cancellationToken)
    {
        var movie = new Models.Movie
        {
            Title = command.Title,
            Description = command.Description,
            TrailerUrl = command.TrailerUrl,
            ReleaseDate = command.ReleaseDate,
            CreatedDate = DateTime.Now
        };

        await repository.CreateAsync(movie);

        await movieService.StoreMoviePoster(movie, command.Image);

        await repository.UpdateAsync(movie);

        return new CreateMovieResult(movie.Id);
    }
}
