namespace Movie.API.Services.Handlers.Genres.Queries.GetGenresByIds;

public record GetGenresByIdsQuery(int[] ids) : IQuery<GetGenresByIdsResult>;

public record GetGenresByIdsResult(ICollection<Genre> Genres);

public class GetGenresByIdsQueryHandler(IGenreRepository repository)
    : IQueryHandler<GetGenresByIdsQuery, GetGenresByIdsResult>
{
    public async Task<GetGenresByIdsResult> Handle(GetGenresByIdsQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(g => query.ids.Contains(g.Id));

        return new GetGenresByIdsResult(result);
    }
}
