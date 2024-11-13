using Movie.BuildingBlocks;
using Movie.WebUI.Models.Dto;
using Movie.WebUI.Services.Abstractions;

namespace Movie.WebUI.Services;

public class TokenProvider(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    public void ClearToken()
    {
        contextAccessor.HttpContext?.Response?.Cookies.Delete(StaticDetails.AccessToken);
        contextAccessor.HttpContext?.Response?.Cookies.Delete(StaticDetails.RefreshToken);
    }

    public TokenDTO? GetToken()
    {
        try
        {
            bool hasAccessToken = contextAccessor
                            .HttpContext
                            .Request
                            .Cookies
                            .TryGetValue(StaticDetails.AccessToken,
                            out string accessToken);

            bool hasRefreshToken = contextAccessor
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

    public void SetToken(TokenDTO token)
    {
        var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(1) };

        contextAccessor.HttpContext?
            .Response
            .Cookies
            .Append(StaticDetails.AccessToken, token.AccessToken, cookieOptions);

        contextAccessor.HttpContext?
            .Response
            .Cookies
            .Append(StaticDetails.RefreshToken, token.RefreshToken, cookieOptions);
    }
}
