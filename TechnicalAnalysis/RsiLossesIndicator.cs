using System;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiLossesIndicator : Indicator
    {
        public RsiLossesIndicator(ITimeSeries timeSeries)
            : base(timeSeries, 1)
        {
            timeSeries.NewDataAvailable += (sender, e) => ClearCachedValues();
        }

        #region Overrides of Indicator

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected override decimal Calculate(DateTime index)
        {
            var currentPeriod = MeasuredTimeSeries[index];
            var previousPeriod = MeasuredTimeSeries.GetPreviousTimePeriods(1, index).First().Value();
            return currentPeriod < previousPeriod ? currentPeriod - previousPeriod : 0;
        }

        #endregion
    }
}