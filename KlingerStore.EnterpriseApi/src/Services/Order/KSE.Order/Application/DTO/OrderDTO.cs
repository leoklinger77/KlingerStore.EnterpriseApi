using System;
using System.Collections.Generic;

namespace KSE.Order.Application.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public int Code { get; set; }
        public int OrderStatus { get; set; }
        public DateTime InsertDate { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherUsed { get; set; }
        public int Installments { get; set; }
        public double Jurus { get; set; }

        public List<OrderItemDTO> Itens { get; set; }
        public ShippingAddressDTO Address { get; set; }        
    }
}
