using System;
using System.Collections.Generic;

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

        public string ClientName { get; set; }
        public string ClientDocument { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }


        public string State { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Zipcode { get; set; }

        public List<OrderStartItems> Itens { get; set; } = new List<OrderStartItems>();
    }

    public class OrderStartItems
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Unit_price { get; set; }
        public int Quantity { get; set; }
        public bool Tangible { get; set; }
    }
}
