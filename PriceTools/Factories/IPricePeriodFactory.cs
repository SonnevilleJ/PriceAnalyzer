using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface IPricePeriodFactory
    {
        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> with data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="tail">The last DateTime of the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        IPricePeriod ConstructStaticPricePeriod(DateTime head, DateTime tail, decimal close, long? volume = null);

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> with data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="resolution">The resolution of the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        IPricePeriod ConstructStaticPricePeriod(DateTime head, Resolution resolution, decimal close, long? volume = null);

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> with data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="tail">The last DateTime of the period.</param>
        /// <param name="open">The per-share price of the first transaction in the period.</param>
        /// <param name="high">The highest per-share price traded in the period.</param>
        /// <param name="low">The lowest per-share price traded in the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        IPricePeriod ConstructStaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null);

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> with data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="resolution">The resolution of the period.</param>
        /// <param name="open">The per-share price of the first transaction in the period.</param>
        /// <param name="high">The highest per-share price traded in the period.</param>
        /// <param name="low">The lowest per-share price traded in the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        IPricePeriod ConstructStaticPricePeriod(DateTime head, Resolution resolution, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null);

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> which aggregates price data from <see cref="IPriceTick"/>s.
        /// </summary>
        /// <returns></returns>
        ITickedPricePeriod ConstructTickedPricePeriod();

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> which aggregates price data from <see cref="IPriceTick"/>s.
        /// </summary>
        /// <param name="priceTicks"></param>
        /// <returns></returns>
        ITickedPricePeriod ConstructTickedPricePeriod(IEnumerable<IPriceTick> priceTicks);

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> which aggregates price data from <see cref="IPriceTick"/>s.
        /// </summary>
        /// <param name="priceTickArray"></param>
        /// <returns></returns>
        ITickedPricePeriod ConstructTickedPricePeriod(params IPriceTick[] priceTickArray);
    }
}