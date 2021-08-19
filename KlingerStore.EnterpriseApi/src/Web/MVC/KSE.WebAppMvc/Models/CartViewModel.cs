using System.Collections.Generic;

namespace KSE.WebAppMvc.Models
{
    public class CartViewModel
    {
        public decimal TotalValue { get; set; }
        public bool VoucherUsed { get; set; }
        public decimal Discount { get; set; }
        public VoucherViewModel Voucher { get; set; }
        public List<CartItemViewModel> Itens { get; set; } = new List<CartItemViewModel>();
    }
}
