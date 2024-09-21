using AutoMapper;
using FluentValidation;
using Movie.BuildingBlocks.CQRS;
using System.Windows.Input;
using static Movie.BuildingBlocks.CQRS.ICommandHandler;

namespace Movie.WebUI.Services.Handlers.Movies.Commands;

public record CreateMovieCommand(string Title, 
    float Rating, 
    string Description, 
    DateOnly ReleaseDate) : ICommand<CreateMovieResult>;

public record CreateMovieResult(int Id, bool IsSuccess);

public class CreateMovieCommandHandler(IMovieService movieService,
    IMapper mapper)
    : ICommandHandler<CreateMovieCommand, CreateMovieResult>
{
    public async Task<CreateMovieResult> Handle(CreateMovieCommand command, 
        CancellationToken cancellationToken)
    {
        var movie = mapper.Map<CreateMovieDto>(command);

        var response = await movieService.CreateMovie(movie);

        if (response != null)
        {
            return new CreateMovieResult(response.Id, true);
        }

        return new CreateMovieResult(int.MinValue, false);
    }
}
