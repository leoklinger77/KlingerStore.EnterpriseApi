using KSE.Core.Mediatr;
using KSE.Order.Application.Querys;
using KSE.Order.Domain.Interfaces;
using KSE.Order.Infrastructure.Data;
using KSE.Order.Infrastructure.Data.Repository;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Order.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services)
        {
            //API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //Application
            services.AddScoped<IMediatrHandler, MediatrHandler>();
            services.AddScoped<IVoucherQuery, VoucherQuery>();

            //Data
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<OrderContext>();           
        }
    }
}
