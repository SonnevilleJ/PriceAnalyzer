using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface ITimeSeriesUtility
    {
        /// <summary>
        /// Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to inspect.</param>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimePeriod has a valid value for the given date.</returns>
        bool HasValueInRange(ITimeSeries timeSeries, DateTime settlementDate);

        /// <summary>
        /// Determines if the IPriceSeries has a valid value for a given date.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to inspect.</param>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the IPricePeriod has a valid value for the given date.</returns>
        bool HasValueInRange(IPriceSeries priceSeries, DateTime settlementDate);

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        IEnumerable<ITimePeriod> ResizeTimePeriods(ITimeSeries timeSeries, Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this TimeSeries.</exception>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        IEnumerable<ITimePeriod> ResizeTimePeriods(ITimeSeries timeSeries, Resolution resolution, DateTime head, DateTime tail);

        /// <summary>
        /// Gets the preceding <see cref="ITimePeriod"/>s previous to an <paramref name="origin" /> date.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="origin">The date of the current period.</param>
        /// <returns></returns>
        ITimePeriod GetPreviousTimePeriod(ITimeSeries timeSeries, DateTime origin);

        /// <summary>
        /// Gets a list of <see cref="ITimePeriod"/>s previous to an <paramref name="origin" /> date.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="maximumCount">The maximum number of periods to select.</param>
        /// <param name="origin">The date which all period tail must precede.</param>
        /// <returns></returns>
        IEnumerable<ITimePeriod> GetPreviousTimePeriods(ITimeSeries timeSeries, int maximumCount, DateTime origin);

        /// <summary>
        /// Gets a list of <see cref="IPricePeriod"/>s previous to an <paramref name="origin" /> date.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="maximumCount">The maximum number of periods to select.</param>
        /// <param name="origin">The date which all period tail must precede.</param>
        /// <returns></returns>
        IEnumerable<IPricePeriod> GetPreviousPricePeriods(IPriceSeries priceSeries, int maximumCount, DateTime origin);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution, DateTime head, DateTime tail);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="pricePeriods"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="pricePeriods"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution, DateTime head, DateTime tail);
    }
}