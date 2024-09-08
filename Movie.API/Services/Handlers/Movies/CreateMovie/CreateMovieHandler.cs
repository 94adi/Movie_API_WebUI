using Movie.API.Repository.Abstractions;
using static Movie.BuildingBlocks.CQRS.ICommandHandler;

namespace Movie.API.Services.Handlers.Movies.CreateMovie;

public record CreateMovieCommand(string Title, float Rating, string Description, DateOnly ReleaseDate) : 
    Movie.BuildingBlocks.CQRS.ICommand<CreateMovieResult>;

public record CreateMovieResult(int Id);


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
