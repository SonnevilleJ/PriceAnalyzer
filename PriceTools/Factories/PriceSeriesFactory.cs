using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs a <see cref="PriceSeries"/> object.
    /// </summary>
    public static class PriceSeriesFactory
    {
        private static readonly object Syncroot = new object();
        private static readonly IDictionary<string, PriceSeries> Dictionary = new Dictionary<string, PriceSeries>();

        /// <summary>
        /// Constructs a <see cref="PriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="PriceSeries"/>.</param>
        /// <returns>The <see cref="PriceSeries"/> for the given ticker.</returns>
        public static PriceSeries CreatePriceSeries(string ticker)
        {
            lock (Syncroot)
            {
                if (!Dictionary.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeries {Ticker = ticker};
                    Dictionary.Add(ticker, priceSeries);
                }
                return Dictionary[ticker];
            }
        }

        /// <summary>
        /// Constructs a <see cref="PriceSeries"/> for the given ticker and resolution.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="PriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="PricePeriod"/>s contained in the <see cref="PriceSeries"/>.</param>
        /// <returns>The <see cref="PriceSeries"/> for the given ticker.</returns>
        public static PriceSeries CreatePriceSeries(string ticker, Resolution resolution)
        {
            lock (Syncroot)
            {
                if (!Dictionary.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeries(resolution) {Ticker = ticker};
                    Dictionary.Add(ticker, priceSeries);
                }
                var existing = Dictionary[ticker];
                if (existing.Resolution > resolution)
                    throw new NotSupportedException("Existing PriceSeries has a resolution larger than requested.");
                return existing;
            }
        }
    }
}