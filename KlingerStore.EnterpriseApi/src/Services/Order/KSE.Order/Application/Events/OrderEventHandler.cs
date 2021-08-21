using KSE.Core.Messages.IntegrationEvents.Order;
using KSE.MessageBus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KSE.Order.Application.Events
{
    public class OrderEventHandler : INotificationHandler<OrderPlacedEvent>
    {
        private readonly IMessageBus _bus;

        public OrderEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(OrderPlacedEvent message, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new OrderPlacedIntegrationEvent(message.ClientId));
        }
    }
}
