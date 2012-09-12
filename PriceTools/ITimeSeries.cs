using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time-series of data.
    /// </summary>
    public interface ITimeSeries : TimePeriod
    {
        /// <summary>
        /// Gets a collection of the <see cref="PricePeriodImpl"/>s in this PriceSeries.
        /// </summary>
        IList<IPricePeriod> PricePeriods { get; }

        /// <summary>
        /// Gets the <see cref="PricePeriodImpl"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="PricePeriodImpl"/> to get.</param>
        /// <returns>The <see cref="PricePeriodImpl"/> stored at the given index.</returns>
        IPricePeriod this[int index] { get; }

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriodImpl"/>s in this PriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="PricePeriodImpl"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods();

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriodImpl"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="PricePeriodImpl"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriodImpl"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="PriceSeries.Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="PricePeriodImpl"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail);

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriod"></param>
        void AddPriceData(IPricePeriod pricePeriod);

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriods"></param>
        void AddPriceData(IEnumerable<IPricePeriod> pricePeriods);
    }
}