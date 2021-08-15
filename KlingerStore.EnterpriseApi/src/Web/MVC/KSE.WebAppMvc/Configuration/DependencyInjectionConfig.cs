using KSE.WebAppMvc.Extensions;
using KSE.WebAppMvc.Extensions.Polly;
using KSE.WebAppMvc.Services;
using KSE.WebAppMvc.Services.Handlers;
using KSE.WebAppMvc.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;

namespace KSE.WebAppMvc.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationHandler>();
            services.AddHttpClient<IAuthService, AuthService>();

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationHandler>()
                .AddPolicyHandler(PollyExtension.WaitAndTry())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            //Refit - Apenas exemplo
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
