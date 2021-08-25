using System;

namespace KSE.Core.Messages.IntegrationEvents.Order
{
    public class OrderRefoundIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }

        public OrderRefoundIntegrationEvent(Guid clientId, Guid orderId)
        {
            ClientId = clientId;
            OrderId = orderId;
        }
    }
}