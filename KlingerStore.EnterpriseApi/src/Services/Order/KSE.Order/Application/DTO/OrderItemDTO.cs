using System;

namespace KSE.Order.Application.DTO
{
    public class OrderItemDTO
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnityValue { get; set; }
        public string ProductImage { get; set; }
    }
}
