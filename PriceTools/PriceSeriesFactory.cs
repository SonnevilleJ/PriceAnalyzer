using System.Data;

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
        public static IPriceSeries CreatePriceSeries(string ticker)
        {
            return CreatePriceSeries(ticker, false);
        }

        /// <summary>
        /// Constructs an <see cref="IPriceSeries"/> for the given ticker.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the IPriceSeries.</param>
        /// <param name="loadFromDatabase"></param>
        /// <returns>The IPriceSeries for the given ticker.</returns>
        private static IPriceSeries CreatePriceSeries(string ticker, bool loadFromDatabase)
        {
            if (loadFromDatabase)
            {
                try
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
                }
                catch (EntityException)
                {
                }
            }
            return new PriceSeries { Ticker = ticker };
        }
    }
}