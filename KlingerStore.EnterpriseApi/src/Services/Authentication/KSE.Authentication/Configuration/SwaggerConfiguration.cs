using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace KSE.Authentication.Configuration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection SwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "KlingerStore Enterprise Identity Api",
                    Description = "Api de Authentication",
                    Contact = new OpenApiContact() {Name="Leandro Klinger", Email = "Leandro.klingeroliveira@gmail.com" },
                    License = new OpenApiLicense() { Name = "Leandro Klinger", Url = new Uri("http://opensource.org/licenses/mit" )},
                });
            });


            return services;
        }

        public static IApplicationBuilder SwaggerApp(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/V1/swagger.json", "V1"));



            return app;
        }
    }
}
