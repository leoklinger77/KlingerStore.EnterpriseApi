using KSE.Gateway.Purchase.Models.Order;
using System.Collections.Generic;

namespace KSE.Gateway.Purchase.Models.Cart
{
    public class CartDTO
    {
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public bool VoucherUsed { get; set; }        
        public VoucherDTO Voucher { get; set; }
        public List<ItemCartDTO> Itens { get; set; } = new List<ItemCartDTO>();
    }
}
