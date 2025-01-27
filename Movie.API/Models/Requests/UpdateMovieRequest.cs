namespace Movie.API.Models.Requests
{
    public class UpdateMovieRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile? Image { get; set; }

        public string TrailerUrl { get; set; }

        public string SelectedGenres { get; set; }

        public DateOnly ReleaseDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LatestUpdateDate { get; set; }
    }
}
