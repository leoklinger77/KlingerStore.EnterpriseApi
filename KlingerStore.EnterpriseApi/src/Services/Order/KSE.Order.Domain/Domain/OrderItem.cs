using KSE.Core.DomainObjets;
using System;

namespace KSE.Order.Domain.Domain
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnityValue { get; private set; }
        public string ProductImage { get; private set; }

        public Order Order { get; set; }
        protected OrderItem() { }

        public OrderItem(Guid productId, string productName, int quantity, decimal unityValue, string productImage = null)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnityValue = unityValue;
            ProductImage = productImage;
        }

        internal decimal CalcValue()
        {
            return Quantity * UnityValue;
        }
    }
}
