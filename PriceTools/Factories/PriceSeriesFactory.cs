using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs a <see cref="IPriceSeries"/> object.
    /// </summary>
    public class PriceSeriesFactory : IPriceSeriesFactory
    {
        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public IPriceSeries ConstructPriceSeries(string ticker)
        {
            return ConstructPriceSeries(ticker, Resolution.Days);
        }

        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker and resolution.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="IPricePeriod"/>s contained in the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        public IPriceSeries ConstructPriceSeries(string ticker, Resolution resolution)
        {
            return new PriceSeries(resolution) {Ticker = ticker};
        }
    }
}