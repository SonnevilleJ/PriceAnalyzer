using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Provides price data for <see cref="IPriceSeries"/>.
    /// </summary>
    public abstract class PriceDataProvider : IPriceDataProvider
    {
        private readonly object _syncroot = new object();
        private readonly IDictionary<IPriceSeries, Task> _tasks = new Dictionary<IPriceSeries, Task>();
        private readonly IDictionary<string, bool> _resetEvent = new Dictionary<string, bool>();

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

        private void AddToUpdateList(IPriceSeries priceSeries)
        {
            var ticker = priceSeries.Ticker;
            bool value;
            if (_resetEvent.TryGetValue(ticker, out value))
            {
                _resetEvent[ticker] = true;
            }
            else
            {
                _resetEvent.Add(ticker, true);
            }
        }

        /// <summary>
        /// Intended to be called asynchronously. Enters a loop which periodically updates the <paramref name="priceSeries"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        private void UpdateLoop(IPriceSeries priceSeries)
        {
            var timeout = new TimeSpan((long) BestResolution);
            lock (priceSeries)
            {
                while (_resetEvent[priceSeries.Ticker])
                {
                    UpdatePriceSeries(priceSeries);

                    Monitor.Wait(priceSeries, timeout);
                }
            }
        }

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
        public abstract IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceDataProvider.</returns>
        public abstract IEnumerable<IPricePeriod> GetPriceData(StockIndex index, DateTime head, DateTime tail, Resolution resolution);
    }
}
