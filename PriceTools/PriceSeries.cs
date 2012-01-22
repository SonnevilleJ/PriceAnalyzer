using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public interface PriceSeries : IPricePeriod
    {
        /// <summary>
        /// Gets the ticker symbol priced by this PriceSeries.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries.
        /// </summary>
        IList<IPricePeriod> PricePeriods { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
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
