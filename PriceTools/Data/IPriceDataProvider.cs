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
        /// Gets a list of <see cref="IPricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <returns></returns>
        IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail);

        /// <summary>
        /// Gets a list of <see cref="IPricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Updates the <paramref name="priceSeries"/> with any missing price data.
        /// </summary>
        /// <param name="priceSeries"></param>
        void UpdatePriceSeries(IPriceSeries priceSeries);

        /// <summary>
        /// Updates the <paramref name="priceSeries"/> with any missing price data.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> containing price history to be updated.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail);

        /// <summary>
        /// Gets a <see cref="IPriceSeries"/> containing price history.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> containing price history to be updated.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Instructs the IPriceDataProvider to periodically update the price data in the <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to update.</param>
        void StartAutoUpdate(IPriceSeries priceSeries);

        /// <summary>
        /// Instructs the IPriceDataProvider to stop periodically updating the price data in <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to stop updating.</param>
        void StopAutoUpdate(IPriceSeries priceSeries);

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        Resolution BestResolution { get; }

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceDataProvider.</returns>
        IEnumerable<IPricePeriod> GetPriceData(StockIndex index, DateTime head, DateTime tail, Resolution resolution);
    }
}