namespace Movie.WebUI.Services.Abstractions;

public interface ITokenProvider
{
    void SetToken(TokenDTO token);
    void SetUserId(string userId);
    TokenDTO? GetToken();
    string GetUserId();
    void ClearToken();
}
