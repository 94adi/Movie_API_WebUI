

namespace Movie.API.Services.User
{
    public class UserService(IUserRepository userRepo) : IUserService
    {
        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await userRepo.GetAsync(u => u.Email == email);

            return user;
        }

        public Task<bool> IsUniqueUser(string userName)
        {
            return userRepo.IsUniqueUser(userName);
        }
    }
}
