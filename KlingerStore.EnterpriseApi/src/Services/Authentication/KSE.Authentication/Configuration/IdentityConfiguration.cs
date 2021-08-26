using Api.Klinger.Extensions;
using KSE.Authentication.Data;
using KSE.WebApi.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.Jwt;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace KSE.Authentication.Configuration
{
    public static class IdentityConfiguration
    {
        public static void IdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwksManager()
                .PersistKeysToDatabaseStore<ApplicationDbContext>();


            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("Connection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMessagePtBr>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddJwtConfiguration(configuration);
        }
    }
}
