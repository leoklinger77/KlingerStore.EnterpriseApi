using KSE.Gateway.Purchase.Extensions;
using KSE.WebApi.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KSE.Gateway.Purchase.Configuration
{
    public static class ApiConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddJwtConfiguration(configuration);

            services.Configure<AppServicesSettings>(configuration);

            services.AddCors(option =>
            {
                option.AddPolicy("Total",
                    builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
        }
        public static void AppWebAppConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("Total");
            app.UseAuthConfiguration();
            app.SwaggerApp();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
