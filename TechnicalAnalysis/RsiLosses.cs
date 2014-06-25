using System;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiLosses : TimeSeriesIndicator
    {
        public RsiLosses(ITimeSeries<ITimePeriod> timeSeries)
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
            var previousPeriodValue = TimeSeriesUtility.GetPreviousTimePeriod(MeasuredTimeSeries, index).Value<decimal>();
            return currentPeriodValue < previousPeriodValue ? currentPeriodValue - previousPeriodValue : 0;
        }
    }
}