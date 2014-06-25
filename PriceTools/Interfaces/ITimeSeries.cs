using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time-series of data.
    /// </summary>
    public interface ITimeSeries<out TSeriesValue, out TPeriodValue> : ITimePeriod<TPeriodValue> where TSeriesValue : ITimePeriod<TPeriodValue>
    {
        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        IEnumerable<TSeriesValue> TimePeriods { get; }
    }
}