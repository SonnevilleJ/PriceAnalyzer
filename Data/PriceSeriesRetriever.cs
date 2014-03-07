using System;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// A class which holds extension methods for <see cref="IPriceSeries"/> classes.
    /// </summary>
    public class PriceSeriesRetriever : IPriceSeriesRetriever
    {
        /// <summary>
        /// Retrieves price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="priceHistoryCsvFileFactory"></param>
        public void UpdatePriceData(IPriceSeries priceSeries, IPriceDataProvider provider, DateTime head, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
        {
            UpdatePriceData(priceSeries, provider, head, DateTime.Now, priceHistoryCsvFileFactory);
        }

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
        public void UpdatePriceData(IPriceSeries priceSeries, IPriceDataProvider provider, DateTime head, DateTime tail, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
        {
            if (priceSeries == null) throw new ArgumentNullException("priceSeries", Strings.PriceSeriesRetrievalExtensions_UpdatePriceData_Paramter_priceSeries_cannot_be_null_);
            if (provider == null) throw new ArgumentNullException("provider", Strings.PriceSeriesRetrievalExtensions_UpdatePriceData_Parameter_provider_cannot_be_null_);
            if (provider.BestResolution > priceSeries.Resolution) throw new ArgumentException(String.Format(Strings.PriceSeriesRetrievalExtensions_UpdatePriceData_Provider_must_be_capable_of_providing_periods_of_resolution__0__or_better_, priceSeries.Resolution), "provider");

            provider.UpdatePriceSeries(priceSeries, head, tail, priceSeries.Resolution, priceHistoryCsvFileFactory);
        }
    }
}
