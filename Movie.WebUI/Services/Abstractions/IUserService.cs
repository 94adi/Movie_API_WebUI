namespace Movie.WebUI.Services.Abstractions
{
    public interface IUserService
    {
        Task<T> LoginAsync<T>(LoginRequestDto loginRequest);
        Task<T> RegisterAsync<T>(RegisterationRequestDto registerRequest);
        Task<T> LogoutAsync<T>(TokenDTO token);
    }
}
