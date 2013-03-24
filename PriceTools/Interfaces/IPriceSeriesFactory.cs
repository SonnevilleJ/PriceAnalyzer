namespace Sonneville.PriceTools
{
    public interface IPriceSeriesFactory
    {
        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        IPriceSeries ConstructPriceSeries(string ticker);

        /// <summary>
        /// Constructs a <see cref="IPriceSeries"/> for the given ticker and resolution.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the <see cref="IPriceSeries"/>.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of the <see cref="IPricePeriod"/>s contained in the <see cref="IPriceSeries"/>.</param>
        /// <returns>The <see cref="IPriceSeries"/> for the given ticker.</returns>
        IPriceSeries ConstructPriceSeries(string ticker, Resolution resolution);
    }
}