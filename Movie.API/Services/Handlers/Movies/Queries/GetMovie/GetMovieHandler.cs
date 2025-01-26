namespace Movie.API.Services.Handlers.Movies.Queries.GetMovie
{
    public record GetMovieQuery(int Id) : IQuery<GetMovieResult>;

    public record GetMovieResult(Models.Dto.MovieDto MovieDto);

    internal class GetMovieHandler(IMovieService movieService,
        IMapper mapper)
        : IQueryHandler<GetMovieQuery, GetMovieResult>
    {
        public async Task<GetMovieResult> Handle(GetMovieQuery query, CancellationToken cancellationToken)
        {
            var result = await movieService.GetByIdAsync(query.Id, true);

            if (result is null)
            {
                throw new NotFoundException("Item was not found");
            }

            await movieService.AddPosterUrls(new List<Models.Movie> { result });

            var movieDto = mapper.Map<Models.Dto.MovieDto>(result);

            return new GetMovieResult(movieDto);
        }
    }
}
