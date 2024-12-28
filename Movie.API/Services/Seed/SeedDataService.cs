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
            Rating = 9.8m,
            Description = "A beautiful movie about hope and friendship",
            ImageUrl = $"{url}//SeedPosters/shawshank.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\shawshank.jpg",
            ReleaseDate = new DateOnly(1994, 2, 12),
            CreatedDate = DateTime.Now
        };

        var movieTheShining = new Models.Movie
        {
            Title = "The Shining",
            Rating = 8.4m,
            Description = "A modern horror masterpiece",
            ImageUrl = $"{url}//SeedPosters/the_shining.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\the_shining.jpg",
            ReleaseDate = new DateOnly(1980, 5, 8),
            CreatedDate = DateTime.Now
        };

        var movieInception = new Models.Movie
        {
            Title = "Inception",
            Rating = 8.8m,
            Description = "A mind-bending thriller",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(2010, 7, 16),
            CreatedDate = DateTime.Now
        };

        var moviePulpFiction = new Models.Movie
        {
            Title = "Pulp Fiction",
            Rating = 8.9m,
            Description = "A cult classic crime film",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(1994, 10, 14),
            CreatedDate = DateTime.Now
        };

        var movieTheGodfather = new Models.Movie
        {
            Title = "The Godfather",
            Rating = 9.2m,
            Description = "An iconic mafia drama",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(1972, 3, 24),
            CreatedDate = DateTime.Now
        };

        var movieInterstellar = new Models.Movie
        {
            Title = "Interstellar",
            Rating = 8.6m,
            Description = "A sci-fi epic exploring space and time",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(2014, 11, 7),
            CreatedDate = DateTime.Now
        };

        var movieFightClub = new Models.Movie
        {
            Title = "Fight Club",
            Rating = 8.8m,
            Description = "A psychological drama with a twist",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(1999, 10, 15),
            CreatedDate = DateTime.Now
        };

        var movieTheDarkKnight = new Models.Movie
        {
            Title = "The Dark Knight",
            Rating = 9.0m,
            Description = "A gritty superhero epic",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(2008, 7, 18),
            CreatedDate = DateTime.Now
        };

        var movieForrestGump = new Models.Movie
        {
            Title = "Forrest Gump",
            Rating = 8.8m,
            Description = "A heartwarming journey through history",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(1994, 7, 6),
            CreatedDate = DateTime.Now
        };

        var movieTheMatrix = new Models.Movie
        {
            Title = "The Matrix",
            Rating = 8.7m,
            Description = "A revolutionary sci-fi action film",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(1999, 3, 31),
            CreatedDate = DateTime.Now
        };

        var movieGoodfellas = new Models.Movie
        {
            Title = "Goodfellas",
            Rating = 8.7m,
            Description = "A gripping mob drama",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
            ReleaseDate = new DateOnly(1990, 9, 19),
            CreatedDate = DateTime.Now
        };

        var movieSchindlersList = new Models.Movie
        {
            Title = "Schindler's List",
            Rating = 9.0m,
            Description = "A powerful Holocaust drama",
            ImageUrl = $"{url}//SeedPosters/default.jpg",
            ImageLocalPath = "wwwroot\\SeedPosters\\default.jpg",
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
                //Rating = new Models.Rating{ RatingValue = 7 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Masterpiece",
                Content = "Absolutely stunning visuals and a captivating story. A must-watch!",
                //Rating = new Models.Rating{ RatingValue = 9 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Not bad",
                Content = "Enjoyed it overall, but felt the pacing was a little slow.",
                //Rating = new Models.Rating{ RatingValue = 6 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Overhyped",
                Content = "I found it a bit predictable and not as thrilling as expected.",
                //Rating = new Models.Rating{ RatingValue = 5 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Emotional rollercoaster",
                Content = "The story really hit me emotionally. Brilliant performances.",
                //Rating = new Models.Rating{ RatingValue = 8 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Good but long",
                Content = "Great character development, but it could have been shorter.",
                //Rating = new Models.Rating{ RatingValue = 7 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Cinematic gem",
                Content = "Visually mesmerizing and well-directed. I was hooked throughout.",
                //Rating = new Models.Rating{ RatingValue = 9 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Fun and entertaining",
                Content = "A lighthearted movie that's great for a weekend watch.",
                //Rating = new Models.Rating{ RatingValue = 7 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Could be better",
                Content = "Had potential but lacked depth in the storyline.",
                //Rating = new Models.Rating{ RatingValue = 5 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Thrilling",
                Content = "Kept me on the edge of my seat! Loved every minute.",
                //Rating = new Models.Rating{ RatingValue = 8 },
                MovieId = 1,
            },
            new Models.Review
            {
                Title = "Forgettable",
                Content = "I watched it, but it didn't leave a lasting impression.",
                //Rating = new Models.Rating{ RatingValue = 6 },
                MovieId = 1,
            }
            });

    }

    private async Task AddMovieReview(IEnumerable<Models.Review> movieReviews)
    {
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