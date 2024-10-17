using Movie.BuildingBlocks;

namespace Movie.API.Services.Seed;

public class SeedDataService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMovieRepository movieRepo) : ISeedDataService
{

    public async Task SeedAsync()
    {
        await AddSeedRoles(new List<IdentityRole>
        {
            new IdentityRole(StaticDetails.userRolesDict[UserRoles.USER]),
            new IdentityRole(StaticDetails.userRolesDict[UserRoles.ADMIN])
        });

        await AddSeedUsers(new List<ApplicationUser>
        {
            new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                RegisteredOn = DateTime.UtcNow
            }

        }, isAdmin: true);

        await AddSeedUsers(new List<ApplicationUser>
        {
            new ApplicationUser
            {
                UserName = "user@user.com",
                Email = "user@user.com",
                EmailConfirmed = true,
                RegisteredOn = DateTime.UtcNow
            }
        }, 
        password:"u$3rp@s$w0rD",
        isAdmin: false);

        await AddMovies(new List<Movie.API.Models.Movie>
        {
           new Movie.API.Models.Movie
           {
               Title = "The Shawshank Redemption",
               Rating = 9.8f,
               Description = "A beautiful movie about hope and friendship",
               ReleaseDate = new DateOnly(1994,2,12),
               CreatedDate = DateTime.Now
           }
        });

    }

    private async Task AddMovies(IEnumerable<Movie.API.Models.Movie> movies)
    {
        if(movies != null && movies.Count() > 0)
        {
            foreach (var movie in movies)
            {
                await movieRepo.CreateAsync(movie);
            }
        }

    }

    private async Task AddSeedRoles(IEnumerable<IdentityRole> roles)
    {
        foreach (var role in roles)
        {
            bool roleAlreadyExists = await roleManager.RoleExistsAsync(role.Name);
            if (!roleAlreadyExists)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private async Task AddSeedUsers(IEnumerable<ApplicationUser> users,
        string password = "$upper@dm1nPwd",
        bool isAdmin = false)
    {
        foreach (var user in users)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email);
            bool userAlreadyExists = existingUser != null ? true : false;

            if (!userAlreadyExists)
            {
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded && isAdmin)
                {
                    await userManager.AddToRoleAsync(user, StaticDetails.userRolesDict[UserRoles.ADMIN]);
                }
                else if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, StaticDetails.userRolesDict[UserRoles.USER]);
                }
            }
        }
    }
}
