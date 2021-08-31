using System;

namespace KSE.Gateway.Purchase.Models.Cart
{
    public class ItemProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public int QuantityStock { get; set; }

    }
}
