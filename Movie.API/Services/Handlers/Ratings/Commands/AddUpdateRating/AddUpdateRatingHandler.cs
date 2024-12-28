using Movie.API.Services.Rating;

namespace Movie.API.Services.Handlers.Ratings.Commands.AddUpdateRating;

public record AddUpdateRatingCommand(Models.Rating Rating) : ICommand<AddUpdateRatingResult>;

public record AddUpdateRatingResult(bool IsSuccess, string ErrorMessage);


public class AddUpdateRatingCommandHandler(IRatingService ratingService)
    : ICommandHandler<AddUpdateRatingCommand, AddUpdateRatingResult>
{
    public async Task<AddUpdateRatingResult> Handle(AddUpdateRatingCommand command, 
        CancellationToken cancellationToken)
    {
        try
        {
            var ratingObj = await ratingService.GetMovieRatingByUser(command.Rating.MovieId, 
                                    command.Rating.UserId);

            if (ratingObj != null && ratingObj.Id > 0)
            {
                ratingObj.RatingValue = command.Rating.RatingValue;
                await ratingService.UpdateRating(ratingObj);
            }
            else
            {
                await ratingService.AddRating(command.Rating);
            }
        }
        catch (Exception ex) 
        { 
            return new AddUpdateRatingResult(false, ex.Message.ToString());
        }

        return new AddUpdateRatingResult(true, "");
    }
}
