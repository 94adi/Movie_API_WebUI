namespace Movie.WebUI.Models.ViewModel;

public class MovieReviewsVM : PagedBaseVM<ReviewDto>
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
}
