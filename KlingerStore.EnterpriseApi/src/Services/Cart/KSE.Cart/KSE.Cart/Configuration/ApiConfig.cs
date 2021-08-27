using KSE.Cart.Extensions;
using KSE.Cart.Services.gRPC;
using KSE.WebApi.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;

namespace KSE.Cart.Configuration
{
    public static class ApiConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddGrpc();

            services.AddJwtConfiguration(configuration);

            services.Configure<BookstoreDatabaseSettings>(
               configuration.GetSection(nameof(BookstoreDatabaseSettings)));

            services.AddCors(option =>
            {
                option.AddPolicy("Total",
                    builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            BsonSerializer.RegisterSerializationProvider(new CustomSerializationProvider());
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
                endpoints.MapGrpcService<CartGrpcService>().RequireCors("Total");
            });
        }
    }
}
