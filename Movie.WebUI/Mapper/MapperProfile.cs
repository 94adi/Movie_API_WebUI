using AutoMapper;
using Movie.WebUI.Services.Handlers.Movies.Commands;
using Movie.WebUI.Services.Handlers.Users.Commands;

namespace Movie.WebUI.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<LoginCommand, LoginRequestDto>();

        CreateMap<RegisterCommand, RegisterationRequestDto>();

        CreateMap<RegisterationRequestDto, RegisterCommand>();

        CreateMap<LoginRequestDto, LoginCommand>();

        CreateMap<LogoutCommand, LogoutRequestDto>();

        CreateMap<CreateMovieCommand, CreateMovieDto>().ReverseMap();

        CreateMap<GetMovieByIdResultDto, MovieDto>();

        CreateMap<UpdateMovieDto, MovieDto>().ReverseMap();

        CreateMap<UpdateMovieCarouselCommand, UpdateMovieCarouselDto>();
    }
}
