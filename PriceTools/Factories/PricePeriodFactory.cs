using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="IPricePeriod"/> objects.
    /// </summary>
    public class PricePeriodFactory : IPricePeriodFactory
    {
        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> with data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="tail">The last DateTime of the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public IPricePeriod ConstructStaticPricePeriod(DateTime head, DateTime tail, decimal close, long? volume = null)
        {
            return ConstructStaticPricePeriod(head, tail, null, null, null, close, volume);
        }

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> with data.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="resolution">The resolution of the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public IPricePeriod ConstructStaticPricePeriod(DateTime head, Resolution resolution, decimal close, long? volume = null)
        {
            return ConstructStaticPricePeriod(head, resolution, null, null, null, close, volume);
        }

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
        public IPricePeriod ConstructStaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null)
        {
            return new StaticPricePeriod(head, tail, open, high, low, close, volume);
        }

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
        public IPricePeriod ConstructStaticPricePeriod(DateTime head, Resolution resolution, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null)
        {
            return new StaticPricePeriod(head, resolution, open, high, low, close, volume);
        }

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> which aggregates price data from <see cref="PriceTick"/>s.
        /// </summary>
        /// <returns></returns>
        public ITickedPricePeriod ConstructTickedPricePeriod()
        {
            return new TickedPricePeriod();
        }

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> which aggregates price data from <see cref="PriceTick"/>s.
        /// </summary>
        /// <param name="priceTicks"></param>
        /// <returns></returns>
        public ITickedPricePeriod ConstructTickedPricePeriod(IEnumerable<PriceTick> priceTicks)
        {
            var period = new TickedPricePeriod();
            period.AddPriceTicks(priceTicks.ToArray());
            return period;
        }

        /// <summary>
        /// Constructs a <see cref="IPricePeriod"/> which aggregates price data from <see cref="PriceTick"/>s.
        /// </summary>
        /// <param name="priceTick"></param>
        /// <returns></returns>
        public ITickedPricePeriod ConstructTickedPricePeriod(PriceTick priceTick)
        {
            return ConstructTickedPricePeriod(new[] {priceTick});
        }
    }
}
