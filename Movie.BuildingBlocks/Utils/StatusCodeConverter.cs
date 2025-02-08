using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Movie.BuildingBlocks.Utils
{
    public static class StatusCodeConverter
    {
        public static HttpStatusCode ConvertToHttpStatusCode(int statusCode)
        {
            return Enum.IsDefined(typeof(HttpStatusCode), statusCode)
                ? (HttpStatusCode)statusCode
                : HttpStatusCode.InternalServerError;
        }
    }
}
