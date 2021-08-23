using KSE.Catalog.Data;
using KSE.Catalog.Interfaces;
using KSE.Catalog.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Catalog.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogContext>(option => option.UseSqlServer(configuration.GetConnectionString("Connection")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CatalogContext>();
        }
    }
}
