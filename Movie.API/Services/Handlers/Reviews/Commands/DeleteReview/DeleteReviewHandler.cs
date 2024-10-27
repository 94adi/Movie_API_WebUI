
namespace Movie.API.Services.Handlers.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(int Id) : ICommand<DeleteReviewResult>;

public record DeleteReviewResult(bool IsSuccess, string errorMesage);


internal class DeleteReviewCommandHandler(IReviewService reviewService)
    : ICommandHandler<DeleteReviewCommand, DeleteReviewResult>
{
    public async Task<DeleteReviewResult> Handle(DeleteReviewCommand command, 
        CancellationToken cancellationToken)
    {
        try
        {
            await reviewService.DeleteReview(command.Id);
        }
        catch (Exception ex) 
        {
            return new DeleteReviewResult(false, ex.ToString());
        }

        return new DeleteReviewResult(true, null);
    }
}
