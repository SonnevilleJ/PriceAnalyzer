namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs a <see cref="PriceSeries"/> object.
    /// </summary>
    public static class PriceSeriesFactory
    {
        /// <summary>
        /// Constructs a <see cref="PriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="PriceSeries"/>.</param>
        /// <returns>The <see cref="PriceSeries"/> for the given ticker.</returns>
        public static PriceSeries CreatePriceSeries(string ticker)
        {
            return new PriceSeries { Ticker = ticker };
        }

        /// <summary>
        /// Constructs a <see cref="PriceSeries"/> for the given ticker and resolution.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="PriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="PricePeriod"/>s contained in the <see cref="PriceSeries"/>.</param>
        /// <returns>The <see cref="PriceSeries"/> for the given ticker.</returns>
        public static PriceSeries CreatePriceSeries(string ticker, Resolution resolution)
        {
            return new PriceSeries(resolution) {Ticker = ticker};
        }
    }
}