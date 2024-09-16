using Movie.WebUI.Models;

namespace Movie.WebUI.Services.Abstractions;

public interface IApiMessageRequestBuilder
{
    HttpRequestMessage Build(ApiRequest apiRequest);
}
