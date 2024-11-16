using System.Collections;
using System.Collections.Generic;

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
        if (apiRequest.ContentType == ContentType.MultipartFormData &&
            apiRequest.Data != null)
        {
            var content = new MultipartFormDataContent();
                foreach (var prop in apiRequest.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(apiRequest.Data);
                    if (value is FormFile)
                    {
                        var formFile = (FormFile)value;
                        using var memoryStream = new MemoryStream();
                        formFile.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        var fileContent = new ByteArrayContent(memoryStream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

                        content.Add(fileContent, prop.Name, formFile.FileName); 
                    }
                    else if(value is IEnumerable<string> stringList)
                    {
                        var jsonContent = JsonConvert.SerializeObject(stringList);
                        content.Add(new StringContent(jsonContent), prop.Name);
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
