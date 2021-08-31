namespace KSE.Cart.Models
{
    public class Voucher
    {
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public string Code { get; set; }
        public TypeDiscountVoucher TypeDiscountVoucher { get; set; }
    }
    public enum TypeDiscountVoucher
    {
        Percentage = 1,
        DiscountValue = 2
    }
}
