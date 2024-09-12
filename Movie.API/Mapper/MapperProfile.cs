using Movie.API.Services.Handlers.Users.Commands.Login;

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

        CreateMap<GetMovieResult, GetMovieResponse>();

        CreateMap<ApplicationUser, RegisterResult>();

        CreateMap<RegisterUserRequest, RegisterCommand>();
        CreateMap<RegisterResult, RegisterUserResponse>();

        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<Token, LoginResponse>().ReverseMap();
    }
}
