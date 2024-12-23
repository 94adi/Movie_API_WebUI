namespace Movie.API.Models.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public int? NoAgree { get; set; }

        public int? NoDisagree { get; set; }

        public int MovieId { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
