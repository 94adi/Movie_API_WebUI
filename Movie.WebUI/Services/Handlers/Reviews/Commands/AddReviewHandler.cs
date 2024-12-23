namespace Movie.WebUI.Services.Handlers.Reviews.Commands;

public record AddReviewCommand(CreateReviewDto Review) : ICommand<AddReviewResult>;

public record AddReviewResult(int Id, bool IsSuccess);

public class AddReviewCommandHandler(IReviewService reviewService)
    : ICommandHandler<AddReviewCommand, AddReviewResult>
{
    public async Task<AddReviewResult> Handle(AddReviewCommand command,
        CancellationToken cancellationToken)
    {
        var response = await reviewService.AddReview(command.Review);

        if (response != null)
        {
            return new AddReviewResult(response.Id, true);
        }

        return new AddReviewResult(int.MinValue, false);
    }
}
