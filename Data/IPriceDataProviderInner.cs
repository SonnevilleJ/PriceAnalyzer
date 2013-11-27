using System;
using System.IO;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Data
{
    public interface IPriceDataProviderInner
    {
        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceDataProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <param name="head">The head of the price data to retrieve.</param>
        /// <param name="tail">The tail of the price data to retrieve.</param>
        /// <param name="impliedResolution">The <see cref="Resolution"/> of price data to retrieve.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail, Resolution? impliedResolution = null);

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        Resolution BestResolution { get; }

        string GetIndexTicker(StockIndex index);
    }
}