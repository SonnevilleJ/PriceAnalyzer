using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="ITimeSeries"/> objects.
    /// </summary>
    public class TimeSeriesFactory : ITimeSeriesFactory
    {
        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <returns></returns>
        public ITimeSeries ConstructMutable()
        {
            return ConstructMutable(new List<ITimePeriod>());
        }

        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        public ITimeSeries ConstructMutable(IEnumerable<ITimePeriod> timePeriods)
        {
            return new TimeSeriesImpl(timePeriods);
        }

        /// <summary>
        /// Constructs an ITimeSeries with an immutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        public ITimeSeries ConstructImmutable(IEnumerable<ITimePeriod> timePeriods)
        {
            
            return new TimeSeriesImpl(new List<ITimePeriod>(timePeriods).AsReadOnly());
        }
    }
}
