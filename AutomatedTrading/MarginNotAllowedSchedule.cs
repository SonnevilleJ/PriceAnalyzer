namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A margin schedule for accounts that do not allow margin.
    /// </summary>
    public class MarginNotAllowedSchedule : IMarginSchedule
    {
        /// <summary>
        /// Gets a value indicating whether or not trading on margin is allowed.
        /// </summary>
        public bool IsMarginAllowed
        {
            get { return false; }
        }

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