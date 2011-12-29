using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    ///   Provides a <see cref="PriceHistoryCsvFile"/> containing price data.
    /// </summary>
    public abstract class PriceDataProvider : IPriceDataProvider
    {
        #region Private Members

        private readonly IDictionary<IPriceSeries, CancellationTokenSource> _tokens = new Dictionary<IPriceSeries, CancellationTokenSource>();
        private readonly IDictionary<IPriceSeries, Task> _tasks = new Dictionary<IPriceSeries, Task>();

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
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to download.</param>
        /// <returns></returns>
        public PriceHistoryCsvFile GetPriceHistoryCsvFile(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            using (var stream = DownloadPricesToCsv(ticker, head, tail, resolution))
            {
                return CreatePriceHistoryCsvFile(stream, head, tail);
            }
        }

        /// <summary>
        /// Instructs the IPriceDataProvider to periodically update the price data in the <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to update.</param>
        public void StartAutoUpdate(IPriceSeries priceSeries)
        {
            if (_tasks.ContainsKey(priceSeries))
            {
                throw new InvalidOperationException("Cannot execute duplicate tasks to update the same IPriceSeries.");
            }
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = new Task(() => UpdateLoop(priceSeries, token), token);
            lock (priceSeries)
            {
                _tokens.Add(priceSeries, cts);
                _tasks.Add(priceSeries, task);
            }

            task.Start();
        }

        /// <summary>
        /// Instructs the IPriceDataProvider to stop periodically updating the price data in <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to stop updating.</param>
        public void StopAutoUpdate(IPriceSeries priceSeries)
        {
            CancellationTokenSource cts;
            if (_tokens.TryGetValue(priceSeries, out cts))
            {
                lock (priceSeries)
                {
                    _tokens.Remove(priceSeries);
                    _tasks.Remove(priceSeries);
                    cts.Cancel();
                    Monitor.PulseAll(priceSeries);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///   Downloads a CSV data file
        /// </summary>
        /// <param name = "ticker">The ticker symbol of the security to price.</param>
        /// <param name = "head">The beginning of the date range to price.</param>
        /// <param name = "tail">The end of the date range to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to download.</param>
        /// <exception cref="WebException">Thrown when accessing the Internet fails.</exception>
        /// <returns>A <see cref = "Stream" /> containing the price data in CSV format.</returns>
        private Stream DownloadPricesToCsv(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            try
            {
                var url = FormUrlQuery(ticker, head, tail, resolution);
                var client = new WebClient {Proxy = {Credentials = CredentialCache.DefaultNetworkCredentials}};
                return client.OpenRead(url);
            }
            catch(WebException e)
            {
                throw new WebException(Strings.DownloadPricesToCsv_InternetAccessFailed, e, e.Status, e.Response);
            }
        }

        /// <summary>
        /// Intended to be called asynchronously. Enters a loop which periodically updates the <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="token"></param>
        private void UpdateLoop(IPriceSeries priceSeries, CancellationToken token)
        {
            var timeout = new TimeSpan((long) BestResolution);
            while (!token.IsCancellationRequested)
            {
                lock (priceSeries)
                {
                    UpdatePriceSeries(priceSeries);

                    Monitor.Wait(priceSeries, timeout);
                }
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
        /// Gets the <see cref="Resolution"/> component of the URL query string used to retrieve price history.
        /// </summary>
        /// <param name="resolution">The <see cref="Resolution"/> to request.</param>
        /// <returns>A partial URL query string containing a marker which requests the given <see cref="Resolution"/>.</returns>
        protected abstract string GetUrlResolution(Resolution resolution);

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
        protected virtual string FormUrlQuery(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            var builder = new StringBuilder();
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
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceDataProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <param name="head">The head of the price data to retrieve.</param>
        /// <param name="tail">The tail of the price data to retrieve.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        protected abstract PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail);

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public abstract Resolution BestResolution { get; }

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceDataProvider.</returns>
        public abstract string GetIndexTicker(StockIndex index);

        /// <summary>
        /// Updates the <paramref name="priceSeries"/> with any missing price data.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to update.</param>
        protected virtual void UpdatePriceSeries(IPriceSeries priceSeries)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
