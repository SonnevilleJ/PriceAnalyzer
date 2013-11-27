using System;
using System.IO;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Google
{
    /// <summary>
    /// Parses an <see cref = "IPriceSeries" /> from Google Finance CSV files.
    /// </summary>
    public sealed class GooglePriceDataProvider : IPriceDataProviderInner
    {
        private CsvPriceDataProvider _provider;

        public GooglePriceDataProvider()
            : this(new WebClientWrapper(), new GooglePriceHistoryQueryUrlBuilder())
        {
        }

        public GooglePriceDataProvider(IWebClient webClient, IPriceHistoryQueryUrlBuilder priceHistoryQueryUrlBuilder)
        {
            _provider = new CsvPriceDataProvider(webClient, priceHistoryQueryUrlBuilder, this);
        }

        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceDataProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <param name="head">The head of the price data to retrieve.</param>
        /// <param name="tail">The tail of the price data to retrieve.</param>
        /// <param name="impliedResolution">The <see cref="Resolution"/> of price data to retrieve.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        public PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail, Resolution? impliedResolution = null)
        {
            return new GooglePriceHistoryCsvFile(stream, head, tail, impliedResolution);
        }

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public Resolution BestResolution
        {
            get { return Resolution.Days; }
        }

        public string GetIndexTicker(StockIndex index)
        {
            throw new NotImplementedException();
        }
    }
}