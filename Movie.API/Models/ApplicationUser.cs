namespace Movie.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime RegisteredOn {  get; set; }

        public ICollection<Review>  Reviews { get; set; }
    }
}
