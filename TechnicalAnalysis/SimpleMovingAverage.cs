using System;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    ///   A moving average indicator using the simple moving average method.
    /// </summary>
    public class SimpleMovingAverage : Indicator
    {
        #region Constructors

        /// <summary>
        ///   Constructs a new Simple Moving Average.
        /// </summary>
        /// <param name = "timeSeries">The <see cref="ITimeSeries"/> containing the data to be averaged.</param>
        /// <param name = "range">The number of periods to average together.</param>
        public SimpleMovingAverage(ITimeSeries timeSeries, int range)
            : base(timeSeries, range)
        {
        }

        #endregion

        #region Overrides of Indicator

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected override decimal Calculate(DateTime index)
        {
            ThrowIfCannotCalculate(index);

            var sum = 0m;
            for (var i = 0; i < Lookback; i++)
            {
                var currentPeriodOpen = index.SeekPeriods(0 - (Lookback - 1 - i), Resolution).CurrentPeriodOpen(Resolution);
                sum += MeasuredTimeSeries[currentPeriodOpen];
            }
            return sum/Lookback;
        }

        #endregion
    }
}
