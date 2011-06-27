using System;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents an object which can retrieve price data from the Internet.
    /// </summary>
    public interface IPriceDataRetriever
    {
        /// <summary>
        /// Downloads price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="head">The first date to retrieve price data for.</param>
        void DownloadPriceData(DateTime head);

        /// <summary>
        /// Downloads price data for the period between the given dates.
        /// </summary>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="tail">The last date to retrieve price data for.</param>
        void DownloadPriceData(DateTime head, DateTime tail);

        /// <summary>
        /// Downloads price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        void DownloadPriceData(PriceSeriesProvider provider, DateTime head);

        /// <summary>
        /// Downloads price data for the period between the given dates.
        /// </summary>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="tail">The last date to retrieve price data for.</param>
        void DownloadPriceData(PriceSeriesProvider provider, DateTime head, DateTime tail);
    }
}