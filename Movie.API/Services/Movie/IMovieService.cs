﻿namespace Movie.API.Services.Movie;

public interface IMovieService
{
    Task StoreMoviePoster(Models.Movie movie, IFormFile poster);

    Task<Models.Movie> GetByIdAsync(int id, bool includeGenres = false);

    Task RemoveMovieGenres(int movieId);

    Task AddMovieGenres(int movieId, IEnumerable<int> genreIds);

    Task<int> GetMoviesCount();
}
