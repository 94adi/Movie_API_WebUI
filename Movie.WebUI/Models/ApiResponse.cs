using System.Net;

namespace Movie.WebUI.Models;

    public class ApiResponse
    {
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
