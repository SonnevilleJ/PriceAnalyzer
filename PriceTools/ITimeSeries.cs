using System;
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

        /// <summary>
        /// Gets the <see cref="ITimePeriod"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="ITimePeriod"/> to get.</param>
        /// <returns>The <see cref="ITimePeriod"/> stored at the given index.</returns>
        ITimePeriod this[int index] { get; }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        IEnumerable<ITimePeriod> GetTimePeriods();

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        IEnumerable<ITimePeriod> GetTimePeriods(Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="ITimeSeries.Resolution"/> of this TimeSeries.</exception>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        IEnumerable<ITimePeriod> GetTimePeriods(Resolution resolution, DateTime head, DateTime tail);
    }
}