using KSE.Cart.Extensions;
using KSE.Cart.Repository;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KSE.Cart.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();            
            services.AddScoped<IAspNetUser, AspNetUser>();
            
            
            services.AddScoped<IBookstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);

            services.AddScoped<ICartRepository, CartRepository>();
        }
    }
}
