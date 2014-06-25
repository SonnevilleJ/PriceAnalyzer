using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="ITimeSeries"/> objects.
    /// </summary>
    public class TimeSeriesFactory<TPeriodValue> : ITimeSeriesFactory<TPeriodValue>
    {
        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <returns></returns>
        public ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructMutable()
        {
            return ConstructMutable(new List<ITimePeriod<TPeriodValue>>());
        }

        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        public ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructMutable(IEnumerable<ITimePeriod<TPeriodValue>> timePeriods)
        {
            return new TimeSeries<TPeriodValue>(timePeriods);
        }

        /// <summary>
        /// Constructs an ITimeSeries with an immutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        public ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructImmutable(IEnumerable<ITimePeriod<TPeriodValue>> timePeriods)
        {

            return new TimeSeries<TPeriodValue>(new List<ITimePeriod<TPeriodValue>>(timePeriods).AsReadOnly());
        }
    }
}
