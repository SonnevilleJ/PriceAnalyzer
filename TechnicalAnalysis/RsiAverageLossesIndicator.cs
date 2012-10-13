using System;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiAverageLossesIndicator : Indicator
    {
        public RsiAverageLossesIndicator(ITimeSeries timeSeries, int lookback)
            : base(timeSeries, lookback)
        {
        }

        #region Overrides of Indicator

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected override decimal Calculate(DateTime index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
