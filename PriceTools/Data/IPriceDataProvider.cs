using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Provides price data.
    /// </summary>
    public interface IPriceDataProvider
    {
        /// <summary>
        /// Gets a list of <see cref="PricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <returns></returns>
        IEnumerable<PricePeriod> GetPricePeriods(string ticker, DateTime head, DateTime tail);

        /// <summary>
        /// Gets a list of <see cref="PricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="PricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        IEnumerable<PricePeriod> GetPricePeriods(string ticker, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Gets an <see cref="PriceSeries"/> containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="PricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        PriceSeries GetPriceSeries(string ticker, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this IPriceDataProvider.</returns>
        string GetIndexTicker(StockIndex index);

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this IPriceDataProvider.
        /// </summary>
        Resolution BestResolution { get; }

        /// <summary>
        /// Instructs the IPriceDataProvider to periodically update the price data in the <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="PriceSeries"/> to update.</param>
        void StartAutoUpdate(PriceSeries priceSeries);

        /// <summary>
        /// Instructs the IPriceDataProvider to stop periodically updating the price data in <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="PriceSeries"/> to stop updating.</param>
        void StopAutoUpdate(PriceSeries priceSeries);
    }
}