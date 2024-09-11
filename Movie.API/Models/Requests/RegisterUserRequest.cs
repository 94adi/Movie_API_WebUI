namespace Movie.API.Models.Requests;

public class RegisterUserRequest
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
