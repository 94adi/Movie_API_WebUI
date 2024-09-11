namespace Movie.API.Repository.Abstractions;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<bool> IsUniqueUser(string userName);
}
