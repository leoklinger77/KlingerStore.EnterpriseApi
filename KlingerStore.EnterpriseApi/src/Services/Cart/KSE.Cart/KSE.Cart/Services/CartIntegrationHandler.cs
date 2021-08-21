using KSE.Cart.Repository;
using KSE.Core.Messages.IntegrationEvents.Order;
using KSE.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Cart.Services
{
    public class CartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CartIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }
        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderPlacedIntegrationEvent>("OrderPlaced", async request => await DeleteCart(request));
        }
        private async Task DeleteCart(OrderPlacedIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ICartRepository>();

            if (await context.GetClient(message.ClientId) != null)
            {
                await context.Delete(message.ClientId);
            }
        }
    }
}
