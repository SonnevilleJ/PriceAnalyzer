﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="PricePeriod"/> objects.
    /// </summary>
    public static class PricePeriodFactory
    {
        /// <summary>
        /// Constructs a <see cref="PricePeriodImpl"/> with static data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="tail">The last DateTime of the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public static PricePeriod CreateStaticPricePeriod(DateTime head, DateTime tail, decimal close, long? volume = null)
        {
            return CreateStaticPricePeriod(head, tail, null, null, null, close, volume);
        }

        /// <summary>
        /// Constructs a <see cref="PricePeriod"/> with static data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="resolution">The resolution of the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public static PricePeriod CreateStaticPricePeriod(DateTime head, Resolution resolution, decimal close, long? volume = null)
        {
            return CreateStaticPricePeriod(head, resolution, null, null, null, close, volume);
        }

        /// <summary>
        /// Constructs a <see cref="PricePeriod"/> with static data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="tail">The last DateTime of the period.</param>
        /// <param name="open">The per-share price of the first transaction in the period.</param>
        /// <param name="high">The highest per-share price traded in the period.</param>
        /// <param name="low">The lowest per-share price traded in the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public static PricePeriod CreateStaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null)
        {
            return new StaticPricePeriodImpl(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        /// Constructs a <see cref="PricePeriod"/> with static data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="resolution">The resolution of the period.</param>
        /// <param name="open">The per-share price of the first transaction in the period.</param>
        /// <param name="high">The highest per-share price traded in the period.</param>
        /// <param name="low">The lowest per-share price traded in the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public static PricePeriod CreateStaticPricePeriod(DateTime head, Resolution resolution, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null)
        {
            return new StaticPricePeriodImpl(head, resolution, open, high, low, close, volume);
        }

        /// <summary>
        /// Constructs a <see cref="PricePeriod"/> which aggregates price data from <see cref="PriceQuote"/>s.
        /// </summary>
        /// <returns></returns>
        public static QuotedPricePeriod ConstructQuotedPricePeriod()
        {
            return new QuotedPricePeriodImpl();
        }

        /// <summary>
        /// Constructs a <see cref="PricePeriod"/> which aggregates price data from <see cref="PriceQuote"/>s.
        /// </summary>
        /// <param name="priceQuotes"></param>
        /// <returns></returns>
        public static QuotedPricePeriod ConstructQuotedPricePeriod(IEnumerable<PriceQuote> priceQuotes)
        {
            var period = new QuotedPricePeriodImpl();
            period.AddPriceQuotes(priceQuotes.ToArray());
            return period;
        }
    }
}
