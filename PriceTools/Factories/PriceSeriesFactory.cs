using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs a <see cref="IPriceSeries"/> object.
    /// </summary>
    public static class PriceSeriesFactory
    {
        private static readonly object Syncroot = new object();
        private static readonly IDictionary<string, IPriceSeries> ExistingPriceSeries = new Dictionary<string, IPriceSeries>();

        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public static IPriceSeries ConstructPriceSeries(string ticker)
        {
            lock (Syncroot)
            {
                if (!ExistingPriceSeries.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeriesImpl {Ticker = ticker};
                    ExistingPriceSeries.Add(ticker, priceSeries);
                }
                return ExistingPriceSeries[ticker];
            }
        }

        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker and resolution.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="IPricePeriod"/>s contained in the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public static IPriceSeries ConstructPriceSeries(string ticker, Resolution resolution)
        {
            lock (Syncroot)
            {
                if (!ExistingPriceSeries.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeriesImpl(resolution) {Ticker = ticker};
                    ExistingPriceSeries.Add(ticker, priceSeries);
                }
                var existing = ExistingPriceSeries[ticker];
                if (existing.Resolution > resolution)
                    throw new NotSupportedException("Existing price series has a resolution larger than requested.");
                return existing;
            }
        }
    }
}