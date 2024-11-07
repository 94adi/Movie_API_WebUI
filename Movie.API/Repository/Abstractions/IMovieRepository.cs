namespace Movie.API.Repository.Abstractions
{
    public interface IMovieRepository : IRepository<Movie.API.Models.Movie>
    {
        Task<Movie.API.Models.Movie> UpdateAsync(Movie.API.Models.Movie movie);

        Task<Movie.API.Models.Movie> GetByIdAsync(int id);

        Task<Movie.API.Models.Movie> GetByIdWithGenreAsync(int id);

        Task<IList<Models.Movie>> GetMoviesWithGenres(int pageNumber = 1, int pageSize = 0);
    }
}
