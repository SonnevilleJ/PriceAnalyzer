using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A schedule used to calculate trading commissions.
    /// </summary>
    public interface ICommissionSchedule
    {
        decimal PriceCheck(Order order);
    }
}