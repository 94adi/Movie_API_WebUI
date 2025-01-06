namespace Movie.WebUI.Models.ViewModel;

public class DetailsVM
{
    public MovieDto Movie { get; set; }
    public int ReviewsCount { get; set; }
    public int? UserRating { get; set; }
}
