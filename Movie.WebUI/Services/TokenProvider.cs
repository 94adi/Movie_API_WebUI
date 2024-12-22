using Microsoft.AspNetCore.Http;

namespace Movie.WebUI.Services;

public class TokenProvider : ITokenProvider
{
    private readonly CookieOptions _cookieOptions;
    private readonly IHttpContextAccessor _contextAccessor;

    public TokenProvider(IHttpContextAccessor contextAccessor)
    {
        _cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(1) };
        _contextAccessor = contextAccessor;
    }

    public void ClearToken()
    {
        _contextAccessor.HttpContext?.Response?.Cookies.Delete(StaticDetails.AccessToken);
        _contextAccessor.HttpContext?.Response?.Cookies.Delete(StaticDetails.RefreshToken);
    }

    public TokenDTO? GetToken()
    {
        try
        {
            bool hasAccessToken = _contextAccessor
                            .HttpContext
                            .Request
                            .Cookies
                            .TryGetValue(StaticDetails.AccessToken,
                            out string accessToken);

            bool hasRefreshToken = _contextAccessor
                    .HttpContext
                    .Request
                    .Cookies
                    .TryGetValue(StaticDetails.RefreshToken, out string refreshToken);

            bool hasToken = (hasAccessToken && hasAccessToken);

            TokenDTO? result = null;

            if (hasToken)
            {
                result = new TokenDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }

            return result;
        }
        catch (Exception) 
        {
            return null;
        }
    }

    public string GetUserId()
    {
        var result = _contextAccessor.HttpContext
                        .Request
                        .Cookies
                        .TryGetValue(StaticDetails.UserId,
                        out string userId);

        return userId;
    }

    public void SetToken(TokenDTO token)
    {
        _contextAccessor.HttpContext?
            .Response
            .Cookies
            .Append(StaticDetails.AccessToken, token.AccessToken, _cookieOptions);

        _contextAccessor.HttpContext?
            .Response
            .Cookies
            .Append(StaticDetails.RefreshToken, token.RefreshToken, _cookieOptions);
    }

    public void SetUserId(string userId)
    {
        _contextAccessor.HttpContext?
        .Response
        .Cookies
        .Append(StaticDetails.UserId, userId, _cookieOptions);
    }
}
