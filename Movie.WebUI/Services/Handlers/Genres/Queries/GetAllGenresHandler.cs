namespace Movie.WebUI.Services.Handlers.Genres.Queries;

public record GetAllGenresQuery() : IQuery<GetAllGenresResult>;

public record GetAllGenresResult(IEnumerable<GenreDto> GenreDtos);

public class GetAllGenresQueryHandler(IGenreService genreService) 
    : IQueryHandler<GetAllGenresQuery, GetAllGenresResult>
{
    public async Task<GetAllGenresResult> Handle(GetAllGenresQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await genreService.GetGenres();

        return new GetAllGenresResult(result.Genres);
    }
}
