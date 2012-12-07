using System;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiLosses : TimeSeriesIndicator
    {
        public RsiLosses(ITimeSeries timeSeries)
            : base(timeSeries, 2)
        {
            timeSeries.NewDataAvailable += (sender, e) => ClearCachedValues();
        }

        #region Overrides of TimeSeriesIndicator

        /// <summary>
        /// Calculates a single value of this TimeSeriesIndicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected override decimal Calculate(DateTime index)
        {
            var currentPeriodValue = MeasuredTimeSeries[index];
            var previousPeriodValue = MeasuredTimeSeries.GetPreviousTimePeriod(index).Value();
            return currentPeriodValue < previousPeriodValue ? currentPeriodValue - previousPeriodValue : 0;
        }

        #endregion
    }
}