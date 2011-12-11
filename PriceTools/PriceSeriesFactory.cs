namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs an IPriceSeries object.
    /// </summary>
    public static class PriceSeriesFactory
    {
        /// <summary>
        /// Constructs an <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the IPriceSeries.</param>
        /// <returns>The IPriceSeries for the given ticker.</returns>
        public static PriceSeries CreatePriceSeries(string ticker)
        {
            return new PriceSeries { Ticker = ticker };
        }
    }
}