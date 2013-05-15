using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Sonneville.PriceTools.Data.Csv
{
    /// <summary>
    /// Provides price data from Comma Separated Values (CSV) data sources.
    /// </summary>
    public abstract class CsvPriceDataProvider : PriceDataProvider
    {
        private readonly IWebClient _webClient;
        private readonly IUrlManager _urlManager;

        protected CsvPriceDataProvider(IWebClient webClient, IUrlManager urlManager)
        {
            _urlManager = urlManager;
            _webClient = webClient;
        }

        #region Overrides of PriceDataProvider

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

        #endregion

        #region Abstract / Virtual Methods

        #region URL Management

        /// <summary>
        /// Formulates a URL that when queried returns a CSV data stream containing the requested price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to request.</param>
        /// <param name="head">The first date to request.</param>
        /// <param name="tail">The last date to request.</param>
        /// <param name="resolution"></param>
        /// <returns>A fully formed URL.</returns>
        private string FormUrlQuery(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            return _urlManager.FormUrlQuery(ticker, head, tail, resolution);
        }

        #endregion

        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceDataProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <param name="head">The head of the price data to retrieve.</param>
        /// <param name="tail">The tail of the price data to retrieve.</param>
        /// <param name="impliedResolution">The <see cref="Resolution"/> of price data to retrieve.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        protected abstract PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail, Resolution? impliedResolution = null);

        #endregion

        #region Private Methods

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
                return CreatePriceHistoryCsvFile(stream, head, tail, resolution);
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
                var url = FormUrlQuery(ticker, head, tail, resolution);
                return _webClient.OpenRead(url);
            }
            catch (WebException e)
            {
                throw new WebException(Strings.DownloadPricesToCsv_InternetAccessFailed, e, e.Status, e.Response);
            }
        }

        #endregion

    }
}