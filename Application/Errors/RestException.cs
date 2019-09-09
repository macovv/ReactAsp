using System.Net;
using System;
namespace Application.Errors
{
    public class RestException : Exception
    {
        public object Errors { get; set; }
        public HttpStatusCode Code { get; private set; }
        public RestException(HttpStatusCode code, object errors = null)
        {
            this.Errors = errors;
            Code = code;
        }
    }
}