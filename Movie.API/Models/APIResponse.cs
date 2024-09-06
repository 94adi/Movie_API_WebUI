using System.Net;

namespace Movie.API.Models
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
