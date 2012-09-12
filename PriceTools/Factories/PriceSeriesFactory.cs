using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs a <see cref="PriceSeriesImpl"/> object.
    /// </summary>
    public static class PriceSeriesFactory
    {
        private static readonly object Syncroot = new object();
        private static readonly IDictionary<string, IPriceSeries> Dictionary = new Dictionary<string, IPriceSeries>();

        /// <summary>
        /// Constructs a <see cref="PriceSeriesImpl"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="PriceSeriesImpl"/>.</param>
        /// <returns>The <see cref="PriceSeriesImpl"/> for the given ticker.</returns>
        public static IPriceSeries CreatePriceSeries(string ticker)
        {
            lock (Syncroot)
            {
                if (!Dictionary.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeriesImpl {Ticker = ticker};
                    Dictionary.Add(ticker, priceSeries);
                }
                return Dictionary[ticker];
            }
        }

        /// <summary>
        /// Constructs a <see cref="PriceSeriesImpl"/> for the given ticker and resolution.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="PriceSeriesImpl"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="PricePeriodImpl"/>s contained in the <see cref="PriceSeriesImpl"/>.</param>
        /// <returns>The <see cref="PriceSeriesImpl"/> for the given ticker.</returns>
        public static IPriceSeries CreatePriceSeries(string ticker, Resolution resolution)
        {
            lock (Syncroot)
            {
                if (!Dictionary.ContainsKey(ticker))
                {
                    var priceSeries = new PriceSeriesImpl(resolution) {Ticker = ticker};
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