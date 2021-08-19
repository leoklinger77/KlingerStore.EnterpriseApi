namespace KSE.Gateway.Purchase.Models.Order
{
    public class VoucherDTO
    {
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public string Code { get; set; }
        public int TypeDiscountVoucher { get; set; }
    }
}
