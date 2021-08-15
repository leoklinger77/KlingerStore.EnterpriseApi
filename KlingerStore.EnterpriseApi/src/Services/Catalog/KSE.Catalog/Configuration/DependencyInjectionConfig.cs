using KSE.Catalog.Data;
using KSE.Catalog.Interfaces;
using KSE.Catalog.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Catalog.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CatalogContext>();
        }
    }
}
