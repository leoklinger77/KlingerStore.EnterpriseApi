namespace KSE.Payment.Models
{
    public class CreditCart
    {
        public string NameCart { get; set; }
        public string NumberCart { get; set; }
        public string ExpirationCart { get; set; }
        public string CVV { get; set; }

        protected CreditCart() { }

        public CreditCart(string nameCart, string numberCart, string expirationCart, string cvv)
        {
            NameCart = nameCart;
            NumberCart = numberCart;
            ExpirationCart = expirationCart;
            CVV = cvv;
        }
    }
}