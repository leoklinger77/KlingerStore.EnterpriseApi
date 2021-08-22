using KSE.Core.DomainObjets;
using System;

namespace KSE.Order.Domain.Domain
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }
        public string Image { get; private set; }

        public Order Order { get; set; }
        protected OrderItem() { }

        public OrderItem(Guid productId, string name, int quantity, decimal value, string image = null)
        {
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            Value = value;
            Image = image;
        }

        internal decimal CalcValue()
        {
            return Quantity * Value;
        }
    }
}
