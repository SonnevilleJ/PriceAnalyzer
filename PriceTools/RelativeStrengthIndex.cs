using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A momentum oscillator which measures the speed of changes in price movements.
    /// </summary>
    public class RelativeStrengthIndex : Indicator
    {
        #region Constructors

        /// <summary>
        /// Constructs a new RSI <see cref="IIndicator"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        public RelativeStrengthIndex(IPriceSeries priceSeries, int lookback = 14)
            : base(priceSeries, lookback)
        {
        }

        #endregion

        #region Overrides of Indicator

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this Indicator for the given period.</returns>
        protected override decimal Calculate(DateTime index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}