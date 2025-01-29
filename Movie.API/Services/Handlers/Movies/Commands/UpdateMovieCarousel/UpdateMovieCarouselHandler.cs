namespace Movie.API.Services.Handlers.Movies.Commands.UpdateMovieCarousel;

public record UpdateMovieCarouselCommand(IEnumerable<string> movieIds) 
    : ICommand<UpdateMovieCarouselResult>;

public record UpdateMovieCarouselResult(bool IsSuccess);

public class UpdateMovieCarouselCommandValidator : AbstractValidator<UpdateMovieCarouselCommand>
{
    public UpdateMovieCarouselCommandValidator()
    {
        RuleFor(m => m.movieIds).NotEmpty().WithMessage("Movie ids collection must not be empty");
    }
}

public class UpdateMovieCarouselCommandHandler(IMovieCarouselRepository movieCarouselRepo)
    : ICommandHandler<UpdateMovieCarouselCommand, UpdateMovieCarouselResult>
{
    public async Task<UpdateMovieCarouselResult> Handle(UpdateMovieCarouselCommand command, 
        CancellationToken cancellationToken)
    {
        try
        {
            var movieRepos = await movieCarouselRepo.GetAllAsync();
            await movieCarouselRepo.RemoveAsync(movieRepos);

            var movieCarouselList = new List<MovieCarousel>();

            foreach (var movieId in command.movieIds)
            {
                var id = Int32.Parse(movieId);
                movieCarouselList.Add(new MovieCarousel { MovieId = id });
            }

            await movieCarouselRepo.CreateAsync(movieCarouselList);

            return new UpdateMovieCarouselResult(true);
        }
        catch (Exception)
        {
            return new UpdateMovieCarouselResult(false);
        }   
    }
}
