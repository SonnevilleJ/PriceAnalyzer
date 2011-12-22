namespace Sonneville.PriceTools.Trading
{
    public interface IMarginSchedule
    {
        double LeverageRequirement { get; }
    }
}