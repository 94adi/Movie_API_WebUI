﻿namespace Movie.WebUI.Services.Handlers.Movies.Commands;

public record UpdateMovieCommand(UpdateMovieDto Movie) : ICommand<UpdateMovieResult>;

public record UpdateMovieResult(bool IsSuccess);

public class UpdateMovieCommandHandler(IMovieService movieService)
    : ICommandHandler<UpdateMovieCommand, UpdateMovieResult>
{
    public async Task<UpdateMovieResult> Handle(UpdateMovieCommand command, 
        CancellationToken cancellationToken)
    {
        command.Movie.LatestUpdateDate = DateTime.UtcNow;
        var result = await movieService.UpdateMovie(command.Movie);

        return new UpdateMovieResult(result.IsSuccess);
    }
}
