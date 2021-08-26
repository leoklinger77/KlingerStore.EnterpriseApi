using Api.Klinger.Configuration;
using KSE.Authentication.Extensions;
using KSE.WebApi.Core.Identity;
using KSE.WebApi.Core.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDevPack.Security.Jwt.AspNetCore;

namespace KSE.Authentication.Configuration
{
    public static class ApiConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.IdentityConfig(configuration);
            services.AddMessageBusConfiguration(configuration);

            services.SwaggerConfig();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<GeneretorToken>();
        }
        public static void AppWebAppConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.SwaggerApp();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseJwksDiscovery();
        }
    }
}
