namespace Movie.API.Services.Handlers.Reviews.Commands.CreateReview;

public record CreateReviewCommand(string Title,
    string Content,
    int MovieId,
    string UserId) : ICommand<CreateReviewResult>;

public record CreateReviewResult(int Id);

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(r => r.MovieId).NotEmpty().WithMessage("Review needs to be associated with a movie");

        RuleFor(r => r.UserId).NotEmpty().WithMessage("Review needs to be associated with a user");

        RuleFor(r => r.Content)
            .MaximumLength(2000)
            .WithMessage("Review content must not exceed 2000 characters");

        RuleFor(r => r.Title)
            .MaximumLength(300)
            .WithMessage("Review title must not exceed 300 characters");
    }
}

internal class CreateReviewHandler(IReviewService reviewService,
    IMapper mapper)
    : ICommandHandler<CreateReviewCommand, CreateReviewResult>
{
    public async Task<CreateReviewResult> Handle(CreateReviewCommand command,
        CancellationToken cancellationToken)
    {

        var review = mapper.Map<Models.Review>(command);

        await reviewService.AddReview(review);

        return new CreateReviewResult(review.Id);
    }
}