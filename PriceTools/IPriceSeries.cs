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
        /// Gets a collection of reaction highs observed in the IPriceSeries.
        /// </summary>
        IEnumerable<KeyValuePair<DateTime, decimal>> ReactionHighs { get; }

        /// <summary>
        /// Gets a collection of reaction lows observed in the IPriceSeries.
        /// </summary>
        IEnumerable<KeyValuePair<DateTime, decimal>> ReactionLows { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this IPriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods();

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this IPriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this IPriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail);
    }
}
