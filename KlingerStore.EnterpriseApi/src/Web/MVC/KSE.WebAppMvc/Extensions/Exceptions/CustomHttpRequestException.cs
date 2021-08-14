using System;
using System.Net;

namespace KSE.WebAppMvc.Extensions.Exceptions
{
    public class CustomHttpRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public CustomHttpRequestException() { }

        public CustomHttpRequestException(string message, System.Exception innerException) : base(message, innerException) { }

        public CustomHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
