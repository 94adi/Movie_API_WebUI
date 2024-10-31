namespace Movie.API.Repository.Abstractions;

public interface IGenreRepository : IRepository<Genre>
{
    Task<Movie.API.Models.Genre> UpdateAsync(Movie.API.Models.Genre genre);
}
