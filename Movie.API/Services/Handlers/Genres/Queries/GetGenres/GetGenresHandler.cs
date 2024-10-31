namespace Movie.API.Services.Handlers.Genres.Queries.GetGenres;

public record GetGenresQuery(int PageNumber = 1, int PageSize = 0) : IQuery<GetGenresResult>;

public class GetGenresResult 
{
    public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
}

internal class GetGenresQueryHandler(IGenreRepository repository)
    : IQueryHandler<GetGenresQuery, GetGenresResult>
{
    public async Task<GetGenresResult> Handle(GetGenresQuery query, 
        CancellationToken cancellationToken)
    {
        var genres = await repository.GetAllAsync(pageSize: query.PageSize,
            pageNumber: query.PageNumber);

        return new GetGenresResult { Genres = genres };
    }
}
