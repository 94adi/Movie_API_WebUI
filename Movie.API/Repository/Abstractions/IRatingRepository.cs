namespace Movie.API.Repository.Abstractions;

public interface IRatingRepository : IRepository<Rating>
{
    public Task UpdateAsync(Rating rating);
}
