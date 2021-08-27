using KSE.Cart.API.Services.gRPC;
using KSE.Gateway.Purchase.Services.gRPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Gateway.Purchase.Configuration
{
    public static class GrpcConfig
    {
        public static void ConfigGrpcService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICartGrpcService, CartGrpcService>();            

            services.AddGrpcClient<CartPurchase.CartPurchaseClient>(options =>
            {
                options.Address = new System.Uri(configuration["CartUrl"]);
            }).AddInterceptor<GrpcServiceInterceptor>();
        }
    }
}
