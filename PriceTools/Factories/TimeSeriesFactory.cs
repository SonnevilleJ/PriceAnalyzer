using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="ITimeSeries"/> objects.
    /// </summary>
    public class TimeSeriesFactory : ITimeSeriesFactory<decimal>
    {
        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <returns></returns>
        public ITimeSeries<ITimePeriod<decimal>, decimal> ConstructMutable()
        {
            return ConstructMutable(new List<ITimePeriod<decimal>>());
        }

        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        public ITimeSeries<ITimePeriod<decimal>, decimal> ConstructMutable(IEnumerable<ITimePeriod<decimal>> timePeriods)
        {
            return new TimeSeries<decimal>(timePeriods);
        }

        /// <summary>
        /// Constructs an ITimeSeries with an immutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        public ITimeSeries<ITimePeriod<decimal>, decimal> ConstructImmutable(IEnumerable<ITimePeriod<decimal>> timePeriods)
        {

            return new TimeSeries<decimal>(new List<ITimePeriod<decimal>>(timePeriods).AsReadOnly());
        }
    }
}
