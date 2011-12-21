namespace Sonneville.PriceTools.Trading
{
    public interface ICommissionSchedule
    {
        decimal PriceCheck(Order order);
    }
}