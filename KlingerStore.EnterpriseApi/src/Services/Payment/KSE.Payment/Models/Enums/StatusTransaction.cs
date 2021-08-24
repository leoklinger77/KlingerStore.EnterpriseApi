namespace KSE.Payment.Models.Enums
{
    public enum StatusTransaction
    {
        authorized = 1,
        paid,
        refused,
        chargedback,
        cancelled
    }
}