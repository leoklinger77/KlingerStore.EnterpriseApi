using Grpc.Core;
using KSE.WebAppMvc.Extensions.Exceptions;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using Refit;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KSE.WebAppMvc.Extensions.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _nest;
        private static IAuthService _authService;
        public ExceptionMiddleware(RequestDelegate nest)
        {
            _nest = nest;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAuthService authService)
        {
            _authService = authService;


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
            catch (BrokenCircuitException)
            {
                HandlerCircuitBreakerExceptionAsync(httpContext);
            }
            catch(RpcException ex)
            {   
                var statusCode = ex.StatusCode switch
                {
                    StatusCode.Internal => 400,
                    StatusCode.Unauthenticated => 401,
                    StatusCode.PermissionDenied => 403,
                    StatusCode.Unimplemented => 404,
                    _ => 500
                };

                HandleRequestExceptionAsync(httpContext, (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode.ToString()));
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                if(_authService.TokenExpiration())
                {
                    if (_authService.RefreshTokenValid().Result)
                    {
                        context.Response.Redirect(context.Request.Path);
                        return;
                    }
                }
                _authService.Logout();
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        private static void HandlerCircuitBreakerExceptionAsync(HttpContext context)
        {
            context.Response.Redirect("/sistema-indisponivel");
        }
    }
}
