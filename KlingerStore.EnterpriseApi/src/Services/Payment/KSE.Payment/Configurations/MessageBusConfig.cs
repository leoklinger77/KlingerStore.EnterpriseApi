using KSE.Core.Tools;
using KSE.MessageBus;
using KSE.Payment.Services.Handler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KSE.Payment.Configuration
{
    public static class MessageBusConfig
    {
        public static void  AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PaymentIntegrationHandler>();
        }
    }
}
