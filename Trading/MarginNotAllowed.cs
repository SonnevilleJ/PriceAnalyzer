namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A margin schedule for accounts that do not allow margin.
    /// </summary>
    public class MarginNotAllowed : IMarginSchedule
    {
        /// <summary>
        /// Gets the leverage requirement for a given ticker, expressed as a percentage.
        /// </summary>
        /// <param name="ticker">The ticker of a marginable trade.</param>
        /// <returns>The leverage requirement for the given <paramref name="ticker"/>.</returns>
        public double GetLeverageRequirement(string ticker)
        {
            return 1.0;
        }
    }
}