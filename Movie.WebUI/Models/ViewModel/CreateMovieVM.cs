namespace Movie.WebUI.Models.ViewModel;

public class CreateMovieVM
{
    public CreateMovieDto MovieDto { get; set; }
    public string SelectedGenre { get; set; }
    public List<SelectListItem> GenreOptions { get; set; }
}
