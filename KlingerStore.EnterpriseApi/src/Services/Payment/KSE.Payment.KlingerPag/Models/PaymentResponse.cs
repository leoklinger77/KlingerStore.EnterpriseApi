namespace KSE.Payment.KlingerPag.Models
{
    public class PaymentResponse
    {
        public TransactionResponse TransactionResponse { get; set; }
        public TransactionError TransactionError { get; set; }
    }
    
}
