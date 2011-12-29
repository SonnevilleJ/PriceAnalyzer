using System;
using System.Linq;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools.Extensions
{
    /// <summary>
    /// A class which holds extension methods for the <see cref="PriceSeries"/> class.
    /// </summary>
    public static class PriceSeriesDownloadExtensions
    {
        /// <summary>
        /// Downloads price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        public static void DownloadPriceData(this PriceSeries priceSeries, PriceSeriesProvider provider, DateTime head)
        {
            DownloadPriceData(priceSeries, provider, head, DateTime.Now);
        }

        /// <summary>
        /// Downloads price data for the period between the given dates.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="tail">The last date to retrieve price data for.</param>
        public static void DownloadPriceData(this PriceSeries priceSeries, PriceSeriesProvider provider, DateTime head, DateTime tail)
        {
            DownloadPriceDataIncludingBuffer(priceSeries, provider, head, tail);
        }

        private static void DownloadPriceDataIncludingBuffer(PriceSeries priceSeries, PriceSeriesProvider provider, DateTime head, DateTime tail)
        {
            if (provider.BestResolution > priceSeries.Resolution) throw new ArgumentException(string.Format("Provider must be capable of providing periods of resolution {0} or better.", priceSeries.Resolution), "provider");
            var toDownload = new TimeSpan(7, 0, 0, 0);
            foreach (var pricePeriod in provider.GetPriceHistoryCsvFile(priceSeries.Ticker, head.Subtract(toDownload), tail, priceSeries.Resolution).PricePeriods.OrderByDescending(period => period.Head))
            {
                priceSeries.AddPricePeriod(pricePeriod);
            }
        }
    }
}
