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
        bool isResultValid = (result != null && result.Genres != null && result.Genres.Any());
        if (isResultValid)
        {
            return new GetAllGenresResult(result.Genres);
        }

        return new GetAllGenresResult(new List<GenreDto>());
    }
}
