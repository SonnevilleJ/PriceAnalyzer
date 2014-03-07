using System;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    ///   A moving average indicator using the simple moving average method.
    /// </summary>
    public class SimpleMovingAverage : TimeSeriesIndicator
    {
        /// <summary>
        ///   Constructs a new Simple Moving Average.
        /// </summary>
        /// <param name = "timeSeries">The <see cref="ITimeSeries"/> containing the data to be averaged.</param>
        /// <param name = "lookback">The number of periods to average together.</param>
        public SimpleMovingAverage(ITimeSeries timeSeries, int lookback)
            : base(timeSeries, lookback)
        {
        }

        /// <summary>
        /// Calculates a single value of this TimeSeriesIndicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected override decimal Calculate(DateTime index)
        {
            return new TimeSeriesUtility().GetPreviousTimePeriods(MeasuredTimeSeries, Lookback, index.CurrentPeriodClose(Resolution).AddTicks(1)).Sum(period => period.Value())/Lookback;
        }
    }
}
