using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time-series of data.
    /// </summary>
    public interface ITimeSeries : TimePeriod
    {
        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries.
        /// </summary>
        IList<PricePeriod> PricePeriods { get; }

        /// <summary>
        /// Gets the <see cref="PricePeriod"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="PricePeriod"/> to get.</param>
        /// <returns>The <see cref="PricePeriod"/> stored at the given index.</returns>
        PricePeriod this[int index] { get; }

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="PricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<PricePeriod> GetPricePeriods();

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="PricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<PricePeriod> GetPricePeriods(Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="PriceSeries.Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="PricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<PricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail);

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriod"></param>
        void AddPriceData(PricePeriod pricePeriod);

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriods"></param>
        void AddPriceData(IEnumerable<PricePeriod> pricePeriods);
    }
}