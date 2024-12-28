using System.Web;

namespace Movie.WebUI.Utils;

public static class YouTubeHelper
{
    public static string ConvertToEmbedUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) 
        {
            return "";
        }

        Uri uri = new Uri(url);

        var query = HttpUtility.ParseQueryString(uri.Query);
        string videoId = query["v"];

        string result = $"https://www.youtube.com/embed/{videoId}";
        return result;
    }
}
