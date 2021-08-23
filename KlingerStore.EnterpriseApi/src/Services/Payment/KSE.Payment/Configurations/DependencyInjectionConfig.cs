using KSE.Payment.Data;
using KSE.Payment.Data.Repository;
using KSE.Payment.Facade;
using KSE.Payment.Facade.Interfaces;
using KSE.Payment.Interfaces;
using KSE.Payment.Services;
using KSE.Payment.Services.Interfaces;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}
