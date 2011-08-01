using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public interface IPriceSeries : IPricePeriod, IPriceDataRetriever
    {
        /// <summary>
        /// Gets the ticker symbol priced by this IPriceSeries.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        IList<IPricePeriod> PricePeriods { get; }

        /// <summary>
        /// Gets or sets the resolution of PricePeriods to retrieve.
        /// </summary>
        PriceSeriesResolution Resolution { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods();

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceSeriesResolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(PriceSeriesResolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceSeriesResolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(PriceSeriesResolution resolution, DateTime head, DateTime tail);
    }
}
