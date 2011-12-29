using System;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    ///   Provides price data.
    /// </summary>
    public interface IPriceDataProvider
    {
        /// <summary>
        /// Gets a <see cref="PriceHistoryCsvFile"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <returns></returns>
        PriceHistoryCsvFile GetPriceHistoryCsvFile(string ticker, DateTime head, DateTime tail);

        /// <summary>
        /// Gets a <see cref="PriceHistoryCsvFile"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to download.</param>
        /// <returns></returns>
        PriceHistoryCsvFile GetPriceHistoryCsvFile(string ticker, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceDataProvider.</returns>
        string GetIndexTicker(StockIndex index);

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        Resolution BestResolution { get; }
    }
}