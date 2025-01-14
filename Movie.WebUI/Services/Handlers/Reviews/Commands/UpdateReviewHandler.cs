namespace Movie.WebUI.Services.Handlers.Reviews.Commands;

public record UpdateReviewCommand(int Id,
    string Title,
    string Content) : ICommand<UpdateReviewResult>;

public record UpdateReviewResult(bool IsSuccess);

public class UpdateReviewCommandHandler(IReviewService reviewService,
    IMapper mapper) 
    : ICommandHandler<UpdateReviewCommand, UpdateReviewResult>
{
    public async Task<UpdateReviewResult> Handle(UpdateReviewCommand command,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateReviewDto>(command);
        var result = await reviewService.UpdateReview(request);

        return new UpdateReviewResult(result.IsSuccess);
    }
}
