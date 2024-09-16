namespace Movie.WebUI.Services;

public class BaseHttpService : IBaseHttpService
{
    private readonly MovieAppConfig _appConfig;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IApiMessageRequestBuilder _apiMessageRequestBuilder;

    public BaseHttpService(IOptions<MovieAppConfig> movieAppConfig,
        IHttpClientFactory httpClientFactory,
        ITokenProvider tokenProvider,
        IHttpContextAccessor contextAccessor,
        IApiMessageRequestBuilder apiMessageRequestBuilder)
    {
        _appConfig = movieAppConfig.Value;
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _contextAccessor = contextAccessor;
        _apiMessageRequestBuilder = apiMessageRequestBuilder;
    }

    public override async Task<T> SendAsync<T>(ApiRequest apiRequest, bool isAuthenticated = true)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var httpMessage = _apiMessageRequestBuilder.Build(apiRequest);

            HttpResponseMessage httpResponse = null;

            if (!isAuthenticated)
            {
                httpResponse = await httpClient.SendAsync(httpMessage);
            }
            else
            {
                httpResponse = await SendRequestWithRefreshToken(httpClient, httpMessage);
            }

            var apiResponse = new ApiResponse
            {
                IsSuccess = false
            };

            try
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                apiResponse.ErrorMessages = httpResponse.StatusCode switch
                {
                    HttpStatusCode.NotFound => new() { "Not Found" },
                    HttpStatusCode.Forbidden => new() { "Access Denied" },
                    HttpStatusCode.Unauthorized => new() { "Unauthorized" },
                    HttpStatusCode.InternalServerError => new() { "Internal Server Error" },
                    _ => null
                };
#pragma warning restore CS8601 // Possible null reference assignment.

                if (apiResponse.ErrorMessages is null)
                {
                    var apiContent = await httpResponse.Content.ReadAsStringAsync();
                    apiResponse.IsSuccess = true;
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiContent);
                }
            }
            catch (Exception e)
            {
                apiResponse.ErrorMessages = new List<string>() { "Error Encountered", e.Message.ToString() };
            }

            var resultString = JsonConvert.SerializeObject(apiResponse);
            var result = JsonConvert.DeserializeObject<T>(resultString);
            return result;
        }
        catch (Movie.BuildingBlocks.Exceptions.AuthenticationException)
        {
            throw;
        }
        catch(Exception e)
        {
            var apiResponse = new ApiResponse
            {
                ErrorMessages = new List<string>() { e.Message.ToString() },
                IsSuccess = false
            };

            var resultString = JsonConvert.SerializeObject(apiResponse);
            var result = JsonConvert.DeserializeObject<T>(resultString);
            return result;
        }
    }

    private async Task<HttpResponseMessage> SendRequestWithRefreshToken(HttpClient httpClient, 
        HttpRequestMessage httpMessage)
    {
        var token = _tokenProvider.GetToken();

        if(token != null && (!string.IsNullOrEmpty(token.AccessToken)))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        try
        {
            var response = await httpClient.SendAsync(httpMessage);
            if (response.IsSuccessStatusCode) 
            {
                return response;
            }

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await InvokeRefreshTokenEndpoint(httpClient, token.AccessToken, token.RefreshToken);
                response = await httpClient.SendAsync(httpMessage);
            }

            return response;
        }
        catch (HttpRequestException httpClientException)
        {
            if (httpClientException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                //refresh token
                await InvokeRefreshTokenEndpoint(httpClient, token.AccessToken, token.RefreshToken);
                //retry request
                return await httpClient.SendAsync(httpMessage);
            }
            //otherwise  throw exception
            throw;
        }
        catch (Movie.BuildingBlocks.Exceptions.AuthenticationException)
        {
            throw;
        }
    }

    private async Task InvokeRefreshTokenEndpoint(HttpClient httpClient, 
        string accessToken, 
        string refreshToken)
    {
        HttpRequestMessage message = new();

        var token = new TokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        message.Headers.Add("Accept", "application/json");
        message.RequestUri = new Uri($"{_appConfig.MovieApiBase}{_appConfig.MovieApiVersion}{_appConfig.RefreshTokenPath}");
        message.Method = HttpMethod.Post;
        message.Content = new StringContent(JsonConvert.SerializeObject(token), 
            Encoding.UTF8, 
            "application/json");

        var response = await httpClient.SendAsync(message);

        var content = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

        if (!apiResponse.IsSuccess)
        {
            await _contextAccessor.HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            throw new Movie.BuildingBlocks.Exceptions.AuthenticationException();
        }

        var tokenString = JsonConvert.SerializeObject(apiResponse.Result);
        var newToken = JsonConvert.DeserializeObject<TokenDTO>(tokenString);


        if(newToken != null && !string.IsNullOrEmpty(newToken.AccessToken))
        {
            await SignInWithNewTokens(newToken);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", 
                token.AccessToken);
        }
        
    }

    private async Task SignInWithNewTokens(TokenDTO token)
    {
        var handler = new JsonWebTokenHandler();
        var jwt = handler.ReadJsonWebToken(token.AccessToken);

        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, 
            jwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, 
            jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
            claimsPrincipal);

        _tokenProvider.SetToken(token);
    }
}
