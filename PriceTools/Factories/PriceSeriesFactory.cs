using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs an <see cref="IPriceSeries"/> object.
    /// </summary>
    public static class PriceSeriesFactory
    {
        /// <summary>
        /// Constructs an <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public static IPriceSeries CreatePriceSeries(string ticker)
        {
            return new PriceSeriesImpl { Ticker = ticker };
        }

        /// <summary>
        /// Constructs an <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="IPricePeriod"/>s contained in the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public static IPriceSeries CreatePriceSeries(string ticker, Resolution resolution)
        {
            return new PriceSeriesImpl(resolution) {Ticker = ticker};
        }
    }
}