namespace Movie.WebUI.Models.ViewModel;

public class CreateReviewVM
{
    public CreateReviewDto ReviewDto { get; set; }

    public CreateRatingDto RatingDto { get; set; }

    public string PageTitle { get; set; }

    public CreateReviewVM()
    {
        ReviewDto = new();
        RatingDto = new();
        PageTitle = "Add review for movie";
    }
}
