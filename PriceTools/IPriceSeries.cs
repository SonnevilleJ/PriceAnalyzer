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
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        IList<IPricePeriod> GetPricePeriods();

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceSeriesResolution"/>.
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        IList<IPricePeriod> GetPricePeriods(PriceSeriesResolution resolution);

        /// <summary>
        /// Gets or sets the resolution of PricePeriods to retrieve.
        /// </summary>
        PriceSeriesResolution Resolution { get; }
    }
}
