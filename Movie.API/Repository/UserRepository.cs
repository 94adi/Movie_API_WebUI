namespace Movie.API.Repository;

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> IsUniqueUser(string userName)
    {
        var user = await GetAsync(u => u.UserName == userName, tracked:false);

        if (user == null) 
        {
            return true;
        }

        return false;
    }
}
