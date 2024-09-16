namespace Movie.WebUI.Models;

public class ApiRequest
{
    public string Url { get; set; }

    public object Data { get; set; }

    public string Token { get; set; }

    public ApiType ApiType { get; set; } = ApiType.GET;

    public ContentType ContentType { get; set; } = ContentType.Json;
}
