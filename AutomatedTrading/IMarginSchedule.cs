namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A schedule used to calculate margin costs.
    /// </summary>
    public interface IMarginSchedule
    {
        /// <summary>
        /// Gets the leverage requirement for a given ticker, expressed as a percentage.
        /// </summary>
        /// <param name="ticker">The ticker of a marginable trade.</param>
        /// <returns>The leverage requirement for the given <paramref name="ticker"/>.</returns>
        double GetLeverageRequirement(string ticker);
    }
}