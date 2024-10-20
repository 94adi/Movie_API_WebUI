
namespace Movie.WebUI.Services;

public class ApiMessageRequestBuilder : IApiMessageRequestBuilder
{
    public HttpRequestMessage Build(ApiRequest apiRequest)
    {
        var message = new HttpRequestMessage();

        if(apiRequest.ContentType == ContentType.MultipartFormData)
        {
            message.Headers.Add("Accpet", "*/*");
        }
        else
        {
            message.Headers.Add("Accept", "application/json");
        }

        message.RequestUri = new Uri(apiRequest.Url);

        AddContentToMessage(apiRequest, message);

        message.Method = apiRequest.ApiType switch
        {
            ApiType.POST => HttpMethod.Post,
            ApiType.PUT => HttpMethod.Put,
            ApiType.DELETE => HttpMethod.Delete,
            _ => HttpMethod.Get
        };

        return message;
    }

    private void AddContentToMessage(ApiRequest apiRequest, HttpRequestMessage message)
    {
        if (apiRequest.ContentType == ContentType.MultipartFormData)
        {
            var content = new MultipartFormDataContent();
                foreach (var prop in apiRequest.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(apiRequest.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), 
                                file.Name, 
                                file.FileName);
                        }
                    }
                    else
                    {
                        string contentValue = (value == null) ? "" : value.ToString();
                        content.Add(new StringContent(contentValue), prop.Name);
                    }
                }
                message.Content = content;            
        }
        else
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                Encoding.UTF8, "application/json");
        }
    }
}
