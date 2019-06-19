namespace CoEco.Core.Ordering.Domain
{
    public enum OrderStatusId
    {
        New = 1,
        Approved = 2,
        Confirmed = 3,
        Active = 4,
        Completed = 5,
        CanceledByRequestingUnit = 6,
        CanceledByLendingUnit = 7
    }
}
