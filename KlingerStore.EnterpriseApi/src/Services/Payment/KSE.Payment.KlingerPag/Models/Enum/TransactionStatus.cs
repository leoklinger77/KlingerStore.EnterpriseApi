namespace KSE.Payment.KlingerPag.Models.Enum
{
    public enum TransactionStatus
    {
        authorized = 1,
        paid,
        refused,
        chargedback,
        cancelled
    }
}
