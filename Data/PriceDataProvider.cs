using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Provides price data for <see cref="IPriceSeries"/>.
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
        /// Gets a list of <see cref="IPricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <returns></returns>
        public IEnumerable<IPricePeriod> GetPricePeriods(string ticker, DateTime head, DateTime tail)
        {
            return GetPricePeriods(ticker, head, tail, BestResolution);
        }

        /// <summary>
        /// Gets a <see cref="PriceHistoryCsvFile"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public IPriceSeries GetPriceSeries(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker, resolution);
            var pricePeriods = GetPricePeriods(ticker, head, tail, resolution);

            priceSeries.AddPriceData(pricePeriods);
            return priceSeries;
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
                var task = _tasks[priceSeries];
                lock (priceSeries)
                {
                    // cleanup
                    _tokens.Remove(priceSeries);
                    _tasks.Remove(priceSeries);

                    // signal cancellation
                    cts.Cancel();

                    // wake up sleeping tasks so they can terminate
                    Monitor.PulseAll(priceSeries);
                }

                // wait for completion and throw exceptions
                task.Wait();
            }
        }

        #endregion

        #region Private Methods

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

        /// <summary>
        /// Updates the <paramref name="priceSeries"/> with any missing price data.
        /// </summary>
        private void UpdatePriceSeries(IPriceSeries priceSeries)
        {
            var head = (priceSeries.PricePeriods.Count > 0) ? priceSeries.Tail.GetFollowingOpen() : DateTime.Now.GetMostRecentOpen();
            var tail = DateTime.Now.GetMostRecentClose();

            priceSeries.RetrievePriceData(this, head, tail);
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public abstract Resolution BestResolution { get; }

        /// <summary>
        /// Gets a list of <see cref="IPricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public abstract IEnumerable<IPricePeriod> GetPricePeriods(string ticker, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceDataProvider.</returns>
        public abstract string GetIndexTicker(StockIndex index);

        #endregion
    }
}
