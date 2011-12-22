namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A margin schedule for accounts that do not allow margin.
    /// </summary>
    public class MarginNotAllowed : IMarginSchedule
    {
        public double LeverageRequirement
        {
            get { return 1.0; }
        }
    }
}