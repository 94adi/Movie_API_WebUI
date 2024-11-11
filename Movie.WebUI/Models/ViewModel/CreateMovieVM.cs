using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Movie.WebUI.Models.ViewModel;

public class CreateMovieVM
{
    public CreateMovieDto MovieDto { get; set; }   

    [BindNever]
    public List<SelectListItem>? GenreOptions { get; set; }
}
