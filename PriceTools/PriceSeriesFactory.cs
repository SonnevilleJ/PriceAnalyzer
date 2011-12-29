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
        public static PriceSeries CreatePriceSeries(string ticker)
        {
            return new PriceSeries { Ticker = ticker };
        }

        /// <summary>
        /// Constructs an <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="IPricePeriod"/>s contained in the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public static PriceSeries CreatePriceSeries(string ticker, Resolution resolution)
        {
            return new PriceSeries(resolution) {Ticker = ticker};
        }
    }
}