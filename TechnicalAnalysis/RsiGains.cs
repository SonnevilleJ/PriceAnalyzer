using System;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiGains : TimeSeriesIndicator
    {
        public RsiGains(ITimeSeries timeSeries)
            : base(timeSeries, 2)
        {
        }

        /// <summary>
        /// Calculates a single value of this TimeSeriesIndicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected override decimal Calculate(DateTime index)
        {
            var currentPeriodValue = MeasuredTimeSeries[index];
            var previousPeriodValue = TimeSeriesUtility.GetPreviousTimePeriod(MeasuredTimeSeries, index).Value();
            return currentPeriodValue > previousPeriodValue ? currentPeriodValue - previousPeriodValue : 0;
        }
    }
}