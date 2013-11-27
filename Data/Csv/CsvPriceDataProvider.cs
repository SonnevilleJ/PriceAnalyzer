using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Sonneville.PriceTools.Data.Csv
{
    /// <summary>
    /// Provides price data from Comma Separated Values (CSV) data sources.
    /// </summary>
    public class CsvPriceDataProvider : PriceDataProvider
    {
        private readonly IWebClient _webClient;
        private readonly IPriceHistoryQueryUrlBuilder _priceHistoryQueryUrlBuilder;
        private readonly IPriceDataProviderInner _innerPriceDataProvider;

        public CsvPriceDataProvider(IWebClient webClient, IPriceHistoryQueryUrlBuilder priceHistoryQueryUrlBuilder, IPriceDataProviderInner innerPriceDataProvider)
        {
            _priceHistoryQueryUrlBuilder = priceHistoryQueryUrlBuilder;
            _innerPriceDataProvider = innerPriceDataProvider;
            _webClient = webClient;
        }

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public override Resolution BestResolution
        {
            get { return _innerPriceDataProvider.BestResolution; }
        }

        /// <summary>
        /// Gets a list of <see cref="IPricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public override IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            return GetPriceHistoryCsvFile(ticker, head, tail, resolution).PricePeriods;
        }

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceDataProvider.</returns>
        public override IEnumerable<IPricePeriod> GetPriceData(StockIndex index, DateTime head, DateTime tail, Resolution resolution)
        {
            return GetPriceData(_innerPriceDataProvider.GetIndexTicker(index), head, tail, resolution);
        }

        /// <summary>
        /// Gets a <see cref="PriceHistoryCsvFile"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        private PriceHistoryCsvFile GetPriceHistoryCsvFile(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            using (var stream = DownloadPricesToCsv(ticker, head, tail, resolution))
            {
                return _innerPriceDataProvider.CreatePriceHistoryCsvFile(stream, head, tail, resolution);
            }
        }

        /// <summary>
        ///   Downloads a CSV data file
        /// </summary>
        /// <param name = "ticker">The ticker symbol of the security to price.</param>
        /// <param name = "head">The beginning of the date range to price.</param>
        /// <param name = "tail">The end of the date range to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <exception cref="WebException">Thrown when accessing the Internet fails.</exception>
        /// <returns>A <see cref = "Stream" /> containing the price data in CSV format.</returns>
        private Stream DownloadPricesToCsv(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            try
            {
                var url = _priceHistoryQueryUrlBuilder.FormPriceHistoryQueryUrl(ticker, head, tail, resolution);
                return _webClient.OpenRead(url);
            }
            catch (WebException e)
            {
                throw new WebException(Strings.DownloadPricesToCsv_InternetAccessFailed, e, e.Status, e.Response);
            }
        }
    }
}