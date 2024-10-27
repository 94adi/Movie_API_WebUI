namespace Movie.API.Services.Handlers.Reviews.Commands.RateReview;

public record RateReviewCommand(string UserId, 
    int ReviewId, 
    ReviewRating ReviewRating) : ICommand<RateReviewResult>;

public record RateReviewResult(bool IsSuccess);


internal class RateReviewCommandHandler(IReviewService reviewService)
    : ICommandHandler<RateReviewCommand, RateReviewResult>
{
    public async Task<RateReviewResult> Handle(RateReviewCommand command, 
        CancellationToken cancellationToken)
    {
        await reviewService.RateReview(command.UserId, 
            command.ReviewId, 
            command.ReviewRating);

        return new RateReviewResult(true);
    }
}
