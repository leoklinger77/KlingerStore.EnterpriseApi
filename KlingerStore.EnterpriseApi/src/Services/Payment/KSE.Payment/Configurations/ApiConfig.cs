using KSE.Payment.Facade;
using KSE.WebApi.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KSE.Payment.Configuration
{
    public static class ApiConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddJwtConfiguration(configuration);

            services.Configure<PaymentConfiguration>(configuration.GetSection("PaymentConfig"));

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
