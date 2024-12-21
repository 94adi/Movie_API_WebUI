namespace Movie.API.Services.Seed;

public class SeedDataService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMovieRepository movieRepo,
        IGenreRepository genreRepo,
        IMovieGenreRepository movieGenreRepo,
        IMovieCarouselRepository movieCarouselRepo) : ISeedDataService
{

    public async Task SeedAsync()
    {
        var url = Utilities.GetAppUrl();

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
        password: "u$3rp@s$w0rD",
        isAdmin: false);

        var genreHorror = new Genre
        {
            Name = "Horror"
        };

        var genreThriller = new Genre
        {
            Name = "Thriller"
        };

        var genreDrama = new Genre
        {
            Name = "Drama"
        };

        var genrePsychologicalHorror = new Genre
        {
            Name = "Psychological Horror"
        };

        await AddGenres(new List<Genre>
        {
        genreHorror,
        genreThriller,
        genreDrama,
        genrePsychologicalHorror
        });

        var movieShawShankRedemption = new Models.Movie
        {
            Title = "The Shawshank Redemption",
            Rating = 9.8f,
            Description = "A beautiful movie about hope and friendship",
            ImageUrl = $"{url}//SeedPosters/shawshank.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\shawshank.jpg",
            ReleaseDate = new DateOnly(1994, 2, 12),
            CreatedDate = DateTime.Now
        };

        var movieTheShining = new Models.Movie
        {
            Title = "The Shining",
            Rating = 8.4f,
            Description = "A modern horror masterpiece",
            ReleaseDate = new DateOnly(1980, 5, 8),
            CreatedDate = DateTime.Now
        };

        await AddMovies(new List<Models.Movie>
        {
            movieShawShankRedemption,
            movieTheShining
        });

        await AddMovieGenres(new List<Models.MovieGenre>
        {
            new Models.MovieGenre {
                GenreId = genreThriller.Id,
                MovieId = movieShawShankRedemption.Id
            },
            new Models.MovieGenre {
                GenreId = genreDrama.Id,
                MovieId = movieShawShankRedemption.Id
            },

            new Models.MovieGenre {
                GenreId = genrePsychologicalHorror.Id,
                MovieId = movieTheShining.Id
            },

        });

        await AddMovieCarousel(new List<Models.MovieCarousel>
        {
            new Models.MovieCarousel
            {
                MovieId = 1
            },
            //new Models.MovieCarousel { 
            //    MovieId = 2
            //}
        });

    }

    private async Task AddMovieCarousel(IEnumerable<Models.MovieCarousel> movieCarousels)
    {
        if(movieCarousels != null && movieCarousels.Count() > 0)
        {
            foreach (var movieCarousel in movieCarousels) 
            { 
                await movieCarouselRepo.CreateAsync(movieCarousel);
            }
        }
    }

    private async Task AddMovies(IEnumerable<Models.Movie> movies)
    {
        if(movies != null && movies.Count() > 0)
        {
            foreach (var movie in movies)
            {
                await movieRepo.CreateAsync(movie);
            }
        }
    }

    private async Task AddMovieGenres(IEnumerable<Models.MovieGenre> movieGenres)
    {
        if (movieGenres != null && movieGenres.Count() > 0)
        {
            foreach (var movieGenre in movieGenres)
            {
                await movieGenreRepo.CreateAsync(movieGenre);
            }
        }
    }

    private async Task AddGenres(IEnumerable<Models.Genre> genres)
    {
        if (genres != null && genres.Count() > 0)
        {
            foreach (var genre in genres)
            {
                await genreRepo.CreateAsync(genre);
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