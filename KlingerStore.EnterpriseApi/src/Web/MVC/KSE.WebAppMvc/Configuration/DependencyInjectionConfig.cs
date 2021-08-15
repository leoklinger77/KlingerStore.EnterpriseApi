using KSE.WebAppMvc.Extensions;
using KSE.WebAppMvc.Services;
using KSE.WebAppMvc.Services.Handlers;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;

namespace KSE.WebAppMvc.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationHandler>();
            services.AddHttpClient<IAuthService, AuthService>();

            var retryWaitPolicy = HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
            },(outcome, timespan,retryCount, context) => 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Tentando pela {retryCount} vez!");
                Console.ForegroundColor = ConsoleColor.White;
            });

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationHandler>()
                //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => System.TimeSpan.FromMilliseconds(600)));
                .AddPolicyHandler(retryWaitPolicy);

            services.AddHttpClient("Refit", options =>
            {
                options.BaseAddress = new System.Uri(configuration.GetSection("CatalogUrl").Value);
            })
                .AddHttpMessageHandler<HttpClientAuthorizationHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, User>();
        }
    }
}
