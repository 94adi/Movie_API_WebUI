namespace Movie.WebUI.Models.ViewModel;

public class UpsertReviewVM
{
    public UpsertReviewDto ReviewDto { get; set; }

    public UpsertRatingDto RatingDto { get; set; }

    public string PageTitle { get; set; }

    public UpsertReviewVM()
    {
        ReviewDto = new();
        RatingDto = new();
        PageTitle = "Add review for movie";
    }
}
