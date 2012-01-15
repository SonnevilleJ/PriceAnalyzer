﻿namespace Sonneville.PriceTools.Analysis
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

        /// <summary>
        ///   Calculates a single value of this MovingAverage.
        /// </summary>
        /// <param name = "index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this MovingAverage for the given period.</returns>
        protected override void Calculate(int index)
        {
            var count = Lookback - 1;
            if (index >= count)
            {
                decimal sum = 0;
                for (var i = index - count; i <= index; i++)
                {
                    sum += IndexedTimeSeriesValues[i];
                }
                Results[index] = sum / Lookback;
            }
        }
    }
}
