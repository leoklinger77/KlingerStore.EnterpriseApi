using KSE.WebAppMvc.Extensions.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Extensions.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _nest;

        public ExceptionMiddleware(RequestDelegate nest)
        {
            _nest = nest;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _nest(httpContext);
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestException httpRequestException)
        {
            if (httpRequestException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)httpRequestException.StatusCode;
        }
    }
}
