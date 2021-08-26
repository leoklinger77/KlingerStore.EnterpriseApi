using KSE.Core.Tools;
using KSE.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Klinger.Configuration
{
    public static class MessageBusConfig
    {
        public static void  AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));            
        }
    }
}
