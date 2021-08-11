using KSE.Authentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Authentication.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection IdentityConfig(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
