﻿using System;
using System.IO;
using System.Net;
using System.Text;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    ///   Parses an <see cref = "IPriceSeries" /> from CSV data for a single ticker symbol.
    /// </summary>
    public abstract class PriceSeriesProvider
    {
        #region Private Members

        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a <see cref="PriceHistoryCsvFile"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <returns></returns>
        public PriceHistoryCsvFile GetPriceHistoryCsvFile(string ticker, DateTime head, DateTime tail)
        {
            return GetPriceHistoryCsvFile(ticker, head, tail, BestResolution);
        }

        /// <summary>
        /// Gets a <see cref="PriceHistoryCsvFile"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> of <see cref="IPricePeriod"/>s to download.</param>
        /// <returns></returns>
        public PriceHistoryCsvFile GetPriceHistoryCsvFile(string ticker, DateTime head, DateTime tail, PriceSeriesResolution resolution)
        {
            using (Stream stream = DownloadPricesToCsv(ticker, head, tail, resolution))
            {
                return CreatePriceHistoryCsvFile(stream, head, tail);
            }
        }

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceSeriesProvider.</returns>
        public abstract string GetIndexTicker(StockIndex index);

        #endregion

        #region Private Methods

        /// <summary>
        ///   Downloads a CSV data file
        /// </summary>
        /// <param name = "ticker">The ticker symbol of the security to price.</param>
        /// <param name = "head">The beginning of the date range to price.</param>
        /// <param name = "tail">The end of the date range to price.</param>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> of <see cref="IPricePeriod"/>s to download.</param>
        /// <exception cref="WebException">Thrown when accessing the Internet fails.</exception>
        /// <returns>A <see cref = "Stream" /> containing the price data in CSV format.</returns>
        private Stream DownloadPricesToCsv(string ticker, DateTime head, DateTime tail, PriceSeriesResolution resolution)
        {
            try
            {
                string url = FormUrlQuery(ticker, head, tail, resolution);
                WebClient client = new WebClient {Proxy = {Credentials = CredentialCache.DefaultNetworkCredentials}};
                return client.OpenRead(url);
            }
            catch(WebException e)
            {
                throw new WebException(Strings.DownloadPricesToCsv_InternetAccessFailed, e, e.Status, e.Response);
            }
        }

        #endregion

        #region Abstract/Virtual Methods

        #region URL Management

        /// <summary>
        /// Gets the base component of the URL used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A URL scheme, host, path, and miscellaneous query string.</returns>
        protected abstract string GetUrlBase();

        /// <summary>
        /// Gets the ticker symbol component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="symbol">The ticker symbol to retrieve.</param>
        /// <returns>A partial URL query string containing the given ticker symbol.</returns>
        protected abstract string GetUrlTicker(string symbol);

        /// <summary>
        /// Gets the beginning date component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="head">The first period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given beginning date.</returns>
        protected abstract string GetUrlHeadDate(DateTime head);

        /// <summary>
        /// Gets the ending date component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="tail">The last period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given ending date.</returns>
        protected abstract string GetUrlTailDate(DateTime tail);

        /// <summary>
        /// Gets the <see cref="PriceSeriesResolution"/> component of the URL query string used to retrieve price history.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> to request.</param>
        /// <returns>A partial URL query string containing a marker which requests the given <see cref="PriceSeriesResolution"/>.</returns>
        protected abstract string GetUrlResolution(PriceSeriesResolution resolution);

        /// <summary>
        /// Gets the dividend component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A partial URL query string containing a marker which requests dividend data.</returns>
        protected abstract string GetUrlDividends();

        /// <summary>
        /// Gets the CSV marker component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A partial URL qery string containing a marker which requests CSV data.</returns>
        protected abstract string GetUrlCsvMarker();

        /// <summary>
        /// Formulates a URL that when queried returns a CSV data stream containing the requested price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to request.</param>
        /// <param name="head">The first date to request.</param>
        /// <param name="tail">The last date to request.</param>
        /// <param name="resolution"></param>
        /// <returns>A fully formed URL.</returns>
        protected virtual string FormUrlQuery(string ticker, DateTime head, DateTime tail, PriceSeriesResolution resolution)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetUrlBase());
            builder.Append(GetUrlTicker(ticker));
            builder.Append(GetUrlHeadDate(head));
            builder.Append(GetUrlTailDate(tail));
            builder.Append(GetUrlResolution(resolution));
            builder.Append(GetUrlCsvMarker());

            return builder.ToString();
        }

        #endregion

        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceSeriesProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <param name="head">The head of the price data to retrieve.</param>
        /// <param name="tail">The tail of the price data to retrieve.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        protected abstract PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail);

        /// <summary>
        /// Gets the smallest <see cref="PriceSeriesResolution"/> available from this PriceSeriesProvider.
        /// </summary>
        public abstract PriceSeriesResolution BestResolution { get; }

        #endregion
    }
}
