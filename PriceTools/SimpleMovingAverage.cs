using System;

namespace Sonneville.PriceTools
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
        /// <param name = "series">The <see cref="IPriceSeries"/> containing the data to be averaged.</param>
        /// <param name = "range">The number of periods to average together.</param>
        public SimpleMovingAverage(IPriceSeries series, int range)
            : base(series, range)
        {
        }

        #endregion

        /// <summary>
        ///   Calculates a single value of this MovingAverage.
        /// </summary>
        /// <param name = "index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this MovingAverage for the given period.</returns>
        protected override decimal Calculate(DateTime index)
        {
            if (!HasValue(index))
            {
                throw new ArgumentOutOfRangeException("index", index,
                                                      "Argument index must be a date within the span of this Indicator.");
            }

            decimal sum = 0;
            for (DateTime i = index.Subtract(new TimeSpan(Range - 1, 0, 0, 0)); i <= index; i = IncrementDate(i))
            {
                sum += PriceSeries[i];
            }
            lock (Padlock)
            {
                return this[index] = sum/Range;
            }
        }

        /// <summary>
        /// Determines if the MovingAverage has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the MovingAverage has a valid value for the given date.</returns>
        public override bool HasValue(DateTime settlementDate)
        {
            return (settlementDate >= Head.AddDays(Range - 1) && settlementDate <= Tail);
        }
    }
}