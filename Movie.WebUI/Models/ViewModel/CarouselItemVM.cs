namespace Movie.WebUI.Models.ViewModel
{
    public class CarouselItemVM
    {
        public IEnumerable<SelectListItem>? MovieOptions { get; set; }

        [Required]
        [MaxLength(3, ErrorMessage = "You can select up to 3 movies.")]
        public IEnumerable<string> SelectedMovieOptions { get; set; }
    }
}
