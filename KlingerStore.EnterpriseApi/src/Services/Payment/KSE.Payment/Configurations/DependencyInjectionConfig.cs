using KSE.Payment.Data;
using KSE.Payment.Data.Repository;
using KSE.Payment.Facade;
using KSE.Payment.Facade.Interfaces;
using KSE.Payment.Interfaces;
using KSE.Payment.KlingerPag.Service;
using KSE.Payment.Services;
using KSE.Payment.Services.Interfaces;
using KSE.WebApi.Core.Polly;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;

namespace KSE.Payment.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentFacade, PaymentCardCreditFacade>();

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<PaymentContext>();


            services.AddHttpClient<IKlingerPagService, KlingerPagService>()                
                .AddPolicyHandler(PollyExtension.WaitAndTry())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        }
    }
}
