using System;

namespace KSE.Core.Messages.IntegrationEvents.Order
{
    public class OrderStartIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; set; }
        public Guid OrderId { get; set; }
        public int TypeOrder { get; set; }
        public decimal Value { get; set; }

        public string NameCard { get; set; }
        public string NumberCard { get; set; }
        public string ExpirationCard { get; set; }
        public string CVV { get; set; }
    }
}
