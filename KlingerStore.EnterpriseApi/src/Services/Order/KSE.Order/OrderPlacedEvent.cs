using KSE.Core.Messages;
using System;

namespace KSE.Order.Application.Events
{
    public class OrderPlacedEvent : Event
    {
        public Guid OrderId { get; private set; }
        public Guid ClientId { get; private set; }

        public OrderPlacedEvent(Guid orderId, Guid clientId)
        {
            AggregateId = orderId;

            OrderId = orderId;
            ClientId = clientId;
        }
    }
}
