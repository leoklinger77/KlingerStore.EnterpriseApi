using System;

namespace KSE.Core.Messages.IntegrationEvents.Order
{
    public class OrderWrittenOffFromStockIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }

        public OrderWrittenOffFromStockIntegrationEvent(Guid clientId, Guid orderId)
        {
            ClientId = clientId;
            OrderId = orderId;
        }
    }
}