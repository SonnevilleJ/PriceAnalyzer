using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface ITimeSeriesFactory
    {
        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <returns></returns>
        ITimeSeries ConstructMutable();

        /// <summary>
        /// Constructs an ITimeSeries with a mutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        ITimeSeries ConstructMutable(IEnumerable<ITimePeriod> timePeriods);

        /// <summary>
        /// Constructs an ITimeSeries with an immutable list of <see cref="ITimePeriod"/>s.
        /// </summary>
        /// <param name="timePeriods">A list of <see cref="ITimePeriod"/>s contained within the <see cref="ITimeSeries"/>. The list will not change.</param>
        /// <returns></returns>
        ITimeSeries ConstructImmutable(IEnumerable<ITimePeriod> timePeriods);
    }
}