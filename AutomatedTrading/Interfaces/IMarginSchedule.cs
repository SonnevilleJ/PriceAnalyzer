namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IMarginSchedule
    {
        bool IsMarginAllowed { get; }

        double GetLeverageRequirement(string ticker);
    }
}