namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A schedule used to calculate trading commissions.
    /// </summary>
    public interface ICommissionSchedule
    {
        decimal PriceCheck(Order order);
    }
}