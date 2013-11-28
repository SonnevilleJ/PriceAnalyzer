using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IPriceDataProviderInner _innerPriceDataProvider;

        /// <summary>
        /// Gets a list of <see cref="IPricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <returns></returns>
        public IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail)
        {
            return GetPriceData(ticker, head, tail, BestResolution);
        }

        /// <summary>
        /// Updates the <paramref name="priceSeries"/> with any missing price data.
        /// </summary>
        /// <param name="priceSeries"></param>
        public void UpdatePriceSeries(IPriceSeries priceSeries)
        {
            var resolution = priceSeries.Resolution;
            var tail = DateTime.Now.PreviousPeriodClose(resolution);
            var head = (priceSeries.PricePeriods.Any()) ? priceSeries.Tail.NextPeriodOpen(resolution) : tail.PreviousPeriodOpen(resolution);

            UpdatePriceSeries(priceSeries, head, tail);
        }

        /// <summary>
        /// Updates the <paramref name="priceSeries"/> with any missing price data.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        public void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail)
        {
            UpdatePriceSeries(priceSeries, head, tail, priceSeries.Resolution);
        }

        /// <summary>
        /// Gets a <see cref="IPriceSeries"/> containing price history.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> containing price history to be updated.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail, Resolution resolution)
        {
            priceSeries.AddPriceData(GetPriceData(priceSeries.Ticker, head, tail, resolution));
        }

        public CsvPriceDataProvider(IWebClient webClient, IPriceHistoryQueryUrlBuilder priceHistoryQueryUrlBuilder, IPriceDataProviderInner innerPriceDataProvider)
        {
            _priceHistoryQueryUrlBuilder = priceHistoryQueryUrlBuilder;
            _innerPriceDataProvider = innerPriceDataProvider;
            _webClient = webClient;
        }

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public Resolution BestResolution
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
        public IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail, Resolution resolution)
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
        public IEnumerable<IPricePeriod> GetPriceData(StockIndex index, DateTime head, DateTime tail, Resolution resolution)
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