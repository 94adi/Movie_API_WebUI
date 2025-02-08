using AutoMapper;
using Movie.BuildingBlocks.CQRS;

namespace Movie.WebUI.Services.Handlers.Movies.Queries;

public record GetMovieByIdQuery(int Id) : IQuery<GetMovieByIdResult>;

public record GetMovieByIdResult(MovieDto Movie);

public class GetMovieByIdQueryHandler(IMovieService movieService)
	: IQueryHandler<GetMovieByIdQuery, GetMovieByIdResult>
{
	public async Task<GetMovieByIdResult> Handle(GetMovieByIdQuery query, 
		CancellationToken cancellationToken)
	{
		var getMovieByResultDto = await movieService.GetMovieById(query.Id);

		if(getMovieByResultDto?.MovieDto == null)
		{
			throw new Exception($"Movie with id {query.Id} could not be found");
		}

        return new GetMovieByIdResult(getMovieByResultDto.MovieDto);
	}
}
