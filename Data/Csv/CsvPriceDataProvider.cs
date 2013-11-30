using System;
using System.IO;
using System.Net;

namespace Sonneville.PriceTools.Data.Csv
{
    /// <summary>
    /// Provides price data from Comma Separated Values (CSV) data sources.
    /// </summary>
    public class CsvPriceDataProvider : IPriceDataProvider
    {
        private readonly IWebClient _webClient;
        private readonly IPriceHistoryQueryUrlBuilder _priceHistoryQueryUrlBuilder;
        public IPriceHistoryCsvFileFactory PriceHistoryCsvFileFactory { get; private set; }

        /// <summary>
        /// Gets a <see cref="IPriceSeries"/> containing price history.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> containing price history to be updated.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <param name="priceHistoryCsvFileFactory"></param>
        /// <returns></returns>
        public void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail, Resolution resolution, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
        {
            var pricePeriods = GetPriceHistoryCsvFile(priceSeries.Ticker, head, tail, resolution, priceHistoryCsvFileFactory).PricePeriods;
            priceSeries.AddPriceData(pricePeriods);
        }

        public CsvPriceDataProvider(IWebClient webClient, IPriceHistoryQueryUrlBuilder priceHistoryQueryUrlBuilder)
        {
            _priceHistoryQueryUrlBuilder = priceHistoryQueryUrlBuilder;
            _webClient = webClient;
        }

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public Resolution BestResolution
        {
            get { return Resolution.Days; }
        }

        /// <summary>
        /// Gets a <see cref="PriceHistoryCsvFile"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <param name="priceHistoryCsvFileFactory"></param>
        /// <returns></returns>
        private PriceHistoryCsvFile GetPriceHistoryCsvFile(string ticker, DateTime head, DateTime tail, Resolution resolution, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
        {
            using (var stream = DownloadPricesToCsv(ticker, head, tail, resolution))
            {
                return priceHistoryCsvFileFactory.CreatePriceHistoryCsvFile(stream, head, tail, resolution);
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