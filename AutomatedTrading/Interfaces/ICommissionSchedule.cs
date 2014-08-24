using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ICommissionSchedule
    {
        decimal PriceCheck(Order order);
    }
}