using FluentValidation;
using Movie.API.Repository.Abstractions;
using static Movie.BuildingBlocks.CQRS.ICommandHandler;

namespace Movie.API.Services.Handlers.Movies.CreateMovie;

public record CreateMovieCommand(string Title, float Rating, string Description, DateOnly ReleaseDate) : 
    Movie.BuildingBlocks.CQRS.ICommand<CreateMovieResult>;

public record CreateMovieResult(int Id);

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(m => m.Title).NotEmpty().WithMessage("Title is required");

        RuleFor(m => m.Rating).NotEmpty()
            .GreaterThanOrEqualTo(1.0f)
            .LessThanOrEqualTo(10.0f)
            .WithMessage("Rating must be between 1.0 and 10.0");

        RuleFor(m => m.Description)
            .MaximumLength(1000)
            .WithMessage("Description must be less than 1000 characters");

        RuleFor(m => m.ReleaseDate).NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Please enter a valid release date");
    }
}


internal class CreateMovieCommandHandler(IMovieRepository repository) : 
    ICommandHandler<CreateMovieCommand, CreateMovieResult>
{
    public async Task<CreateMovieResult> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
    {
        var movie = new Movie.API.Models.Movie
        {
            Title = command.Title,
            Rating = command.Rating,
            Description = command.Description,
            ReleaseDate = command.ReleaseDate,
            CreatedDate = DateTime.Now
        };

        await repository.CreateAsync(movie);

        return new CreateMovieResult(movie.Id);
    }
}
