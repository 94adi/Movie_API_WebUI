using Movie.WebUI.Models.Dto;

namespace Movie.WebUI.Services.Abstractions;

public interface ITokenProvider
{
    void SetToken(TokenDTO token);
    TokenDTO? GetToken();
    void ClearToken();
}
