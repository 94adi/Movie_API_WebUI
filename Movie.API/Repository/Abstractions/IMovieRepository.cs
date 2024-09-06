﻿namespace Movie.API.Repository.Abstractions
{
    public interface IMovieRepository : IRepository<Movie.API.Models.Movie>
    {
        Task<Movie.API.Models.Movie> UpdateAsync(Movie.API.Models.Movie movie);
    }
}
