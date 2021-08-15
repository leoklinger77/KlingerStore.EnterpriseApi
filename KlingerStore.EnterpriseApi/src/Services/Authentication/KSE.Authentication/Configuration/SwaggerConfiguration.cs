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
                s.SwaggerDoc("V1", new OpenApiInfo()
                {
                    Title = "KlingerStore Enterprise Authentication Api",
                    Version = "V1",
                    Description = "Api Authentication",
                    Contact = new OpenApiContact() { Email = "leandro.klingeroliveira@gmail.com", Name = "Leandro Klinger" },
                    TermsOfService = new Uri("https://opensource.org/licenses/MIT"),
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
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
