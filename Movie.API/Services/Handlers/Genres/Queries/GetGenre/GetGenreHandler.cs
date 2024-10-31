
namespace Movie.API.Services.Handlers.Genres.Queries.GetGenre;

public record GetGenreQuery(int Id) : IQuery<GetGenreResult>;

public record GetGenreResult(Genre Genre);


public class GetGenreQueryHandler(IGenreRepository repository)
    : IQueryHandler<GetGenreQuery, GetGenreResult>
{
    public async Task<GetGenreResult> Handle(GetGenreQuery query, 
        CancellationToken cancellationToken)
    {
        var genre = await repository.GetAsync(g => g.Id == query.Id);

        if(genre == null)
        {
            throw new NotFoundException("Item was not found");
        }

        return new GetGenreResult(genre);
    }
}
