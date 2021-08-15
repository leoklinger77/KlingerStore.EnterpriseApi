using KSE.Catalog.Configuration;
using KSE.Catalog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KSE.Catalog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(option => option.UseSqlServer(Configuration.GetConnectionString("Connection")));            

            services.AddWebAppConfig(Configuration);
            services.SwaggerConfig();
            services.RegisterService();
            
        }       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AppWebAppConfig(env);            
        }
    }
}
