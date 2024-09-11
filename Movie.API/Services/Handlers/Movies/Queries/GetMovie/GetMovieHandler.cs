using Movie.API.Repository.Abstractions;
using Movie.BuildingBlocks.CQRS;

namespace Movie.API.Services.Handlers.Movies.Queries.GetMovie
{
    public record GetMovieQuery(int Id) : IQuery<GetMovieResult>;

    public record GetMovieResult(Models.Movie movie);


    internal class GetMovieHandler(IMovieRepository repository)
        : IQueryHandler<GetMovieQuery, GetMovieResult>
    {
        public async Task<GetMovieResult> Handle(GetMovieQuery query, CancellationToken cancellationToken)
        {
            var movie = await repository.GetByIdAsync(query.Id);

            if (movie is null)
            {
                throw new Exception("Item was not found");
            }

            return new GetMovieResult(movie);
        }
    }
}
