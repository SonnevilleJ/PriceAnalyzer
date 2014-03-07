using System;

namespace Sonneville.PriceTools.Data
{
    public interface IPriceSeriesRetriever
    {
        /// <summary>
        /// Retrieves price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="priceHistoryCsvFileFactory"></param>
        void UpdatePriceData(IPriceSeries priceSeries, IPriceDataProvider provider, DateTime head, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory);

        /// <summary>
        /// Retrieves price data for the period between the given dates.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="tail">The last date to retrieve price data for.</param>
        /// <param name="priceHistoryCsvFileFactory"></param>
        /// <exception cref="ArgumentException">The best available <see cref="Resolution"/> offered by <paramref name="provider"/> is not sufficient for the <see cref="Resolution"/> required by <paramref name="priceSeries"/>.</exception>
        /// <exception cref="ArgumentNullException">A parameter is equal to null.</exception>
        void UpdatePriceData(IPriceSeries priceSeries, IPriceDataProvider provider, DateTime head, DateTime tail, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory);
    }
}