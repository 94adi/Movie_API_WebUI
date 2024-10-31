using Movie.API.Services.Handlers.Genres.Commands.CreateGenre;
using Movie.API.Services.Handlers.Genres.Commands.UpdateGenre;
using Movie.API.Services.Handlers.Genres.Queries.GetGenres;

namespace Movie.API.Mapper;

public class MapperProfile : Profile
{

    public MapperProfile() 
    {
        CreateMap<GetMoviesRequest, GetMoviesQuery>();
        CreateMap<GetMoviesResult, GetMoviesResponse>();

        CreateMap<GetMoviesQuery, Pagination>();
        CreateMap<GetMoviesRequest, Pagination>();

        CreateMap<CreateMovieRequest, CreateMovieCommand>();
        CreateMap<CreateMovieResult, CreateMovieResponse>();

        CreateMap<UpdateMovieCommand, Models.Movie>();
        CreateMap<UpdateMovieRequest, UpdateMovieCommand>();

        CreateMap<GetMovieResult, GetMovieResponse>();

        CreateMap<ApplicationUser, RegisterResult>();

        CreateMap<RegisterUserRequest, RegisterCommand>();
        CreateMap<RegisterResult, RegisterUserResponse>();

        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<Token, LoginResponse>().ReverseMap();

        CreateMap<RefreshTokenRequest, RefreshAccessTokenCommand>();
        CreateMap<RefreshAccessTokenResult, RefreshTokenResponse>();

        CreateMap<RevokeTokenRequest, RevokeTokenCommand>();

        CreateMap<CreateMovieRequest, CreateMovieCommand>();

        CreateMap<CreateReviewCommand, Models.Review>();

        CreateMap<CreateReviewRequest, CreateReviewCommand>();
        CreateMap<CreateReviewResult,  CreateReviewResponse>();

        CreateMap<Models.Review, Models.Dto.ReviewDto>().ReverseMap();

        CreateMap<UpdateGenreCommand,  Models.Genre>();

        CreateMap<CreateGenreResult, CreateGenreResponse>();
        CreateMap<GetGenresResult, GetGenresResponse>();
        CreateMap<GetGenresQuery, Pagination>();

        CreateMap<Models.Movie, Models.Dto.MovieDto>()
            .ForMember(dest => dest.Genres, opt => opt.Ignore());
    }
}
