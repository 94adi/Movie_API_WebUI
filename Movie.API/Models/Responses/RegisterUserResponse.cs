namespace Movie.API.Models.Responses;

public class RegisterUserResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public bool IsSuccess { get; set; }
    public IEnumerable<string> ErrorMessages { get; set; }
}
