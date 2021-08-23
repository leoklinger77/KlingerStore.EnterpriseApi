using KSE.Core.DomainObjets;
using KSE.Core.Messages.IntegrationEvents.Order;
using KSE.MessageBus;
using KSE.Order.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Order.Services
{
    public class PedidoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PedidoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderCanceledIntegrationEvent>("PedidoCancelado",
                async request => await CancelarPedido(request));

            _bus.SubscribeAsync<OrderAuthorizeIntegrationEvent>("PedidoPago",
               async request => await FinalizarPedido(request));
        }

        private async Task CancelarPedido(OrderCanceledIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pedidoRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var pedido = await pedidoRepository.GetById(message.OrderId);
                pedido.CanceledOrder();

                pedidoRepository.Update(pedido);

                if (!await pedidoRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"Problemas ao cancelar o pedido {message.OrderId}");
                }
            }
        }

        private async Task FinalizarPedido(OrderAuthorizeIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pedidoRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var pedido = await pedidoRepository.GetById(message.OrderId);
                pedido.FinishOrder();

                pedidoRepository.Update(pedido);

                if (!await pedidoRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"Problemas ao finalizar o pedido {message.OrderId}");
                }
            }
        }
    }
}