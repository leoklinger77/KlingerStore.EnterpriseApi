namespace KSE.Payment.KlingerPag.Models.Enum
{
    public enum TransactionStatus
    {
        Authorized = 1,
        Paid,
        Refused,
        Chargedback,
        Cancelled
    }
}
