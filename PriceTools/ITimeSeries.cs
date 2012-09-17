using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time-series of data.
    /// </summary>
    public interface ITimeSeries : ITimePeriod
    {
        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        IEnumerable<ITimePeriod> TimePeriods { get; }
    }
}