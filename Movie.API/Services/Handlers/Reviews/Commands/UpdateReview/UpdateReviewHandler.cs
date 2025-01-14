namespace Movie.API.Services.Handlers.Reviews.Commands.UpdateReview;

public record UpdateReviewCommand(int Id,
    string Title,
    string Content) : ICommand<UpdateReviewResult>;

public record UpdateReviewResult(bool IsSuccess);

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(r => r.Content)
            .MaximumLength(2000)
            .WithMessage("Review content must not exceed 2000 characters");

        RuleFor(r => r.Title)
            .MaximumLength(300)
            .WithMessage("Review title must not exceed 300 characters");
    }
}

public class UpdateReviewCommandHandler(IReviewService reviewService,
    IMapper mapper)
    : ICommandHandler<UpdateReviewCommand, UpdateReviewResult>
{
    public async Task<UpdateReviewResult> Handle(UpdateReviewCommand command,
        CancellationToken cancellationToken)
    {
        var currentReview = await reviewService.GetReviewById(command.Id);

        if(currentReview == null)
        {
            return new UpdateReviewResult(false);
        }

        currentReview.Title = command.Title;
        currentReview.Content = command.Content;

        await reviewService.UpdateReview(currentReview);

        return new UpdateReviewResult(true);
    }
}
