namespace Movie.API.Services.Seed;

public class SeedDataService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMovieRepository movieRepo,
        IGenreRepository genreRepo,
        IMovieGenreRepository movieGenreRepo,
        IMovieCarouselRepository movieCarouselRepo,
        IReviewRepository reviewRepository
        ) : ISeedDataService
{

    public async Task SeedAsync()
    {
        string url = Utilities.GetAppUrl();

        await AddSeedRoles(new List<IdentityRole>
        {
            new IdentityRole(StaticDetails.userRolesDict[UserRoles.USER]),
            new IdentityRole(StaticDetails.userRolesDict[UserRoles.ADMIN])
        });

        //store passwords [admin/user] in azure secrets vault
        await AddSeedUsers(new List<ApplicationUser>
        {
            new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                RegisteredOn = DateTime.UtcNow
            },
            new ApplicationUser
            {
                UserName = "creator@admin.com",
                Email = "creator@admin.com",
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

        var creatorUser = await userManager.FindByEmailAsync("creator@admin.com");

        if (creatorUser == null)
            throw new Exception("Creator user could not be created");

        var creatorUserId = creatorUser.Id;

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

        var genreComedy = new Genre
        {
            Name = "Comedy"
        };

        var genreSciFi = new Genre
        {
            Name = "SciFi"
        };


        await AddGenres(new List<Genre>
        {
        genreHorror,
        genreThriller,
        genreDrama,
        genrePsychologicalHorror,
        genreComedy,
        genreSciFi
        });

        var movieShawShankRedemption = new Models.Movie
        {
            Title = "The Shawshank Redemption",
            Description = "A beautiful movie about hope and friendship",
            ImageUrl = url + "/LocalPosters/shawshank.jpg",
            ImageFileName = "shawshank.jpg",
            ReleaseDate = new DateOnly(1994, 2, 12),
            CreatedDate = DateTime.Now
        };

        var movieTheShining = new Models.Movie
        {
            Title = "The Shining",
            Description = "A modern horror masterpiece",
            ImageUrl = url + "/LocalPosters/the_shining.jpg",
            ImageFileName = "the_shining.jpg",
            ReleaseDate = new DateOnly(1980, 5, 8),
            CreatedDate = DateTime.Now
        };

        var movieInception = new Models.Movie
        {
            Title = "Inception",
            Description = "A mind-bending thriller",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(2010, 7, 16),
            CreatedDate = DateTime.Now
        };

        var moviePulpFiction = new Models.Movie
        {
            Title = "Pulp Fiction",
            Description = "A cult classic crime film",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(1994, 10, 14),
            CreatedDate = DateTime.Now
        };

        var movieTheGodfather = new Models.Movie
        {
            Title = "The Godfather",
            Description = "An iconic mafia drama",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(1972, 3, 24),
            CreatedDate = DateTime.Now
        };

        var movieInterstellar = new Models.Movie
        {
            Title = "Interstellar",
            Description = "A sci-fi epic exploring space and time",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(2014, 11, 7),
            CreatedDate = DateTime.Now
        };

        var movieFightClub = new Models.Movie
        {
            Title = "Fight Club",
            Description = "A psychological drama with a twist",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(1999, 10, 15),
            CreatedDate = DateTime.Now
        };

        var movieTheDarkKnight = new Models.Movie
        {
            Title = "The Dark Knight",
            Description = "A gritty superhero epic",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(2008, 7, 18),
            CreatedDate = DateTime.Now
        };

        var movieForrestGump = new Models.Movie
        {
            Title = "Forrest Gump",
            Description = "A heartwarming journey through history",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(1994, 7, 6),
            CreatedDate = DateTime.Now
        };

        var movieTheMatrix = new Models.Movie
        {
            Title = "The Matrix",
            Description = "A revolutionary sci-fi action film",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(1999, 3, 31),
            CreatedDate = DateTime.Now
        };

        var movieGoodfellas = new Models.Movie
        {
            Title = "Goodfellas",
            Description = "A gripping mob drama",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(1990, 9, 19),
            CreatedDate = DateTime.Now
        };

        var movieSchindlersList = new Models.Movie
        {
            Title = "Schindler's List",
            Description = "A powerful Holocaust drama",
            ImageUrl = url + "/LocalPosters/default.jpg",
            ImageFileName = "default.jpg",
            ReleaseDate = new DateOnly(1993, 12, 15),
            CreatedDate = DateTime.Now
        };

        await AddMovies(new List<Models.Movie>
        {
            movieShawShankRedemption,
            movieTheShining,
            movieInception,
            moviePulpFiction,
            movieTheGodfather,
            movieInterstellar,
            movieFightClub,
            movieTheDarkKnight,
            movieForrestGump,
            movieTheMatrix,
            movieGoodfellas,
            movieSchindlersList
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

            new Models.MovieGenre { 
                GenreId = genreThriller.Id, 
                MovieId = movieInception.Id },

            new Models.MovieGenre { GenreId = genreDrama.Id, MovieId = moviePulpFiction.Id },
            new Models.MovieGenre { GenreId = genreDrama.Id, MovieId = movieTheGodfather.Id },
            new Models.MovieGenre { GenreId = genreSciFi.Id, MovieId = movieInterstellar.Id },
            new Models.MovieGenre { GenreId = genreDrama.Id, MovieId = movieFightClub.Id },
            new Models.MovieGenre { GenreId = genreThriller.Id, MovieId = movieTheDarkKnight.Id },
            new Models.MovieGenre { GenreId = genreDrama.Id, MovieId = movieForrestGump.Id },
            new Models.MovieGenre { GenreId = genreSciFi.Id, MovieId = movieTheMatrix.Id },
            new Models.MovieGenre { GenreId = genreDrama.Id, MovieId = movieGoodfellas.Id },
            new Models.MovieGenre { GenreId = genreDrama.Id, MovieId = movieSchindlersList.Id }

        });

        await AddMovieCarousel(new List<Models.MovieCarousel>
        {
            new Models.MovieCarousel
            {
                MovieId = 1
            },
            new Models.MovieCarousel {
                MovieId = 2
            }
        });

        
        await AddMovieReview(new List<Models.Review>
        {
            
            new Models.Review
            {
                Title = "Great movie",
                Content = "I really enjoyed this movie",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Masterpiece",
                Content = "Absolutely stunning visuals and a captivating story. A must-watch!",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Not bad",
                Content = "Enjoyed it overall, but felt the pacing was a little slow.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Overhyped",
                Content = "I found it a bit predictable and not as thrilling as expected.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Emotional rollercoaster",
                Content = "The story really hit me emotionally. Brilliant performances.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Good but long",
                Content = "Great character development, but it could have been shorter.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Cinematic gem",
                Content = "Visually mesmerizing and well-directed. I was hooked throughout.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Fun and entertaining",
                Content = "A lighthearted movie that's great for a weekend watch.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Could be better",
                Content = "Had potential but lacked depth in the storyline.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Thrilling",
                Content = "Kept me on the edge of my seat! Loved every minute.",
                UserId = creatorUserId,
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Forgettable",
                Content = "I watched it, but it didn't leave a lasting impression.",
                UserId = creatorUserId,
                MovieId = 1,
            }
            });
    }

    private async Task AddMovieReview(IEnumerable<Models.Review> movieReviews)
    {
        var existingMovieReviews = await reviewRepository.GetAllAsync();

        if(existingMovieReviews != null && existingMovieReviews.Any())
        {
            return;
        }

        var userId = userManager.FindByEmailAsync("user@user.com").GetAwaiter().GetResult().Id;

        if (movieReviews != null && movieReviews.Count() > 0)
        {
            foreach (var movieReview in movieReviews)
            {
                movieReview.UserId = userId;
                movieReview.CreatedAt = DateTime.Now;
            }
            await reviewRepository.CreateAsync(movieReviews);
        }
    }

    private async Task AddMovieCarousel(IEnumerable<Models.MovieCarousel> movieCarousels)
    {
        var existingMovieCarousels = await movieCarouselRepo.GetAllAsync();
        if(existingMovieCarousels != null && existingMovieCarousels.Any())
        {
            return;
        }

        if (movieCarousels != null && movieCarousels.Count() > 0)
        {
            foreach (var movieCarousel in movieCarousels) 
            { 
                await movieCarouselRepo.CreateAsync(movieCarousel);
            }
        }
    }

    private async Task AddMovies(IEnumerable<Models.Movie> movies)
    {
        var existingMovies = await movieRepo.GetAllAsync();
        if (existingMovies != null && existingMovies.Any())
        {
            return;
        }

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
        var existingMovieGenres = await movieGenreRepo.GetAllAsync();
        if(existingMovieGenres != null && existingMovieGenres.Any())
        {
            return;
        }

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
        var existingGenres = await genreRepo.GetAllAsync();
        if (existingGenres != null && existingGenres.Any())
        {
            return;
        }

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