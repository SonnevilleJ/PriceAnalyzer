namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// Indicates the relative price of two securities.
    /// </summary>
    public class PriceRelative : Indicator
    {
        /// <summary>
        /// Constructs an Indicator for a given <see cref="ITimeSeries"/>.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        public PriceRelative(ITimeSeries timeSeries, int lookback) : base(timeSeries, lookback)
        {
        }
    }
}
