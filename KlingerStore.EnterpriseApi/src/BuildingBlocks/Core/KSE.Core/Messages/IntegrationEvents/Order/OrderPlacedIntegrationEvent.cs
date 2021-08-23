using System;

namespace KSE.Core.Messages.IntegrationEvents.Order
{
    public class OrderPlacedIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; private set; }

        public OrderPlacedIntegrationEvent(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}