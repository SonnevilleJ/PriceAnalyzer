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
        /// <param name="loadFromDatabase"></param>
        /// <returns>The IPriceSeries for the given ticker.</returns>
        public static PriceSeries CreatePriceSeries(string ticker, bool loadFromDatabase = false)
        {
            return loadFromDatabase ? LoadPriceSeries(ticker) : GetEmptyPriceSeries(ticker);
        }

        private static PriceSeries LoadPriceSeries(string ticker)
        {
            using (var db = new Container())
            {
                foreach (var period in db.PricePeriods)
                {
                    PriceSeries series = period as PriceSeries;
                    if (series != null && series.Ticker == ticker)
                    {
                        return series;
                    }
                }
            }
            return GetEmptyPriceSeries(ticker);
        }

        private static PriceSeries GetEmptyPriceSeries(string ticker)
        {
            return new PriceSeries { Ticker = ticker };
        }
    }
}