namespace Movie.API.Services.User;

public interface IUserService
{
    Task<bool> IsUniqueUser(string userName);

    Task<ApplicationUser> GetUserByEmail(string email);
}
