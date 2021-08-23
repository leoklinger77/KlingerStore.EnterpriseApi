using System;
using System.Collections.Generic;

namespace KSE.Core.Messages.IntegrationEvents.Order
{
    public class OrderAuthorizeIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public IDictionary<Guid, int> Itens { get; private set; }

        public OrderAuthorizeIntegrationEvent(Guid clientId, Guid orderId, IDictionary<Guid, int> itens)
        {
            ClientId = clientId;
            OrderId = orderId;
            Itens = itens;
        }
    }
}