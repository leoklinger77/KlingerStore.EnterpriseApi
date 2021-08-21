namespace KSE.Order.Domain.Domain.Enumerations
{
    public enum OrderStatus : int
    {
        Authorized = 1,
        PaidOut = 2,
        refused = 3,
        Delivered = 4,
        canceled = 5
    }
}
