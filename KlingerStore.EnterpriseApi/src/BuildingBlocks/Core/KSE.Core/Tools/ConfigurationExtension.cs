using Microsoft.Extensions.Configuration;

namespace KSE.Core.Tools
{
    public static  class ConfigurationExtension
    {
        public static string GetMessageQueueConnection(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection("MessageQueueConnection")?[name];
        }
    }
}
