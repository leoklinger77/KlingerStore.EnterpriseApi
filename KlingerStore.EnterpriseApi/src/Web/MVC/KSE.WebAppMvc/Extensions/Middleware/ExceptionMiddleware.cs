using KSE.WebAppMvc.Extensions.Exceptions;
using Microsoft.AspNetCore.Http;
using Refit;
using System.Net;
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
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ValidationApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }
    }
}
