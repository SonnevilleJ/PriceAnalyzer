using System;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// Indicates the relative price of two securities.
    /// </summary>
    public class PriceRelative : Indicator
    {
        /// <summary>
        /// Constructs an Indicator for a given <see cref="IPriceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        public PriceRelative(IPriceSeries priceSeries, int lookback) : base(priceSeries, lookback)
        {
        }

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected override decimal? Calculate(int index)
        {
            throw new NotImplementedException();
        }
    }
}
