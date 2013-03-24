using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs a <see cref="IPriceSeries"/> object.
    /// </summary>
    public class PriceSeriesFactory : IPriceSeriesFactory
    {
        private readonly object _syncroot = new object();
        private readonly IDictionary<string, IPriceSeries> _existingPriceSeries = new Dictionary<string, IPriceSeries>();

        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public IPriceSeries ConstructPriceSeries(string ticker)
        {
            lock (_syncroot)
            {
                if (!_existingPriceSeries.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeriesImpl {Ticker = ticker};
                    _existingPriceSeries.Add(ticker, priceSeries);
                }
                return _existingPriceSeries[ticker];
            }
        }

        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker and resolution.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="IPricePeriod"/>s contained in the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public IPriceSeries ConstructPriceSeries(string ticker, Resolution resolution)
        {
            lock (_syncroot)
            {
                if (!_existingPriceSeries.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeriesImpl(resolution) {Ticker = ticker};
                    _existingPriceSeries.Add(ticker, priceSeries);
                }
                var existing = _existingPriceSeries[ticker];
                if (existing.Resolution > resolution)
                    throw new NotSupportedException("Existing price series has a resolution larger than requested.");
                return existing;
            }
        }
    }
}