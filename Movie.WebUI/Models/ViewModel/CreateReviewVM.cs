namespace Movie.WebUI.Models.ViewModel;

public class CreateReviewVM
{
    public CreateReviewDto ReviewDto { get; set; }
    public string PageTitle { get; set; }

    public CreateReviewVM()
    {
        ReviewDto = new CreateReviewDto();
        PageTitle = "Add review for movie";
    }
}
