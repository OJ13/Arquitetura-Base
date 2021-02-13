using System.Collections.Generic;
using System.Net;

namespace API.Models.Response
{
    public class ApiResponseError : ApiResponseBase
    {
        public HttpStatusCode Code { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponseError(HttpStatusCode code, string message)
            : base(message, false)
        {
            Code = code;
            Errors = new List<string>();
        }
    }
}
