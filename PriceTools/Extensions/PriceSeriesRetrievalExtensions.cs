﻿using System;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Extensions
{
    /// <summary>
    /// A class which holds extension methods for the <see cref="PriceSeries"/> class.
    /// </summary>
    public static class PriceSeriesRetrievalExtensions
    {
        /// <summary>
        /// Retrieves price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        public static void RetrievePriceData(this IPriceSeries priceSeries, IPriceDataProvider provider, DateTime head)
        {
            RetrievePriceData(priceSeries, provider, head, DateTime.Now);
        }

        /// <summary>
        /// Retrieves price data for the period between the given dates.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="tail">The last date to retrieve price data for.</param>
        public static void RetrievePriceData(this IPriceSeries priceSeries, IPriceDataProvider provider, DateTime head, DateTime tail)
        {
            if (provider.BestResolution > priceSeries.Resolution) throw new ArgumentException(string.Format("Provider must be capable of providing periods of resolution {0} or better.", priceSeries.Resolution), "provider");
            var pricePeriods = provider.GetPricePeriods(priceSeries.Ticker, head, tail, priceSeries.Resolution);
            priceSeries.AddPriceData(pricePeriods);
        }
    }
}