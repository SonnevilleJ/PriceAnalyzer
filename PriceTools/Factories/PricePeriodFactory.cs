using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs PricePeriod objects.
    /// </summary>
    public static class PricePeriodFactory
    {
        /// <summary>
        /// Constructs a StaticPricePeriod object.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="tail">The last DateTime of the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public static IPricePeriod CreateStaticPricePeriod(DateTime head, DateTime tail, decimal close, long? volume = null)
        {
            return CreateStaticPricePeriod(head, tail, null, null, null, close, volume);
        }

        /// <summary>
        /// Constructs a StaticPricePeriod object.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="tail">The last DateTime of the period.</param>
        /// <param name="open">The per-share price of the first transaction in the period.</param>
        /// <param name="high">The highest per-share price traded in the period.</param>
        /// <param name="low">The lowest per-share price traded in the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public static IPricePeriod CreateStaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null)
        {
            return new StaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        /// Constructs a StaticPricePeriod object.
        /// </summary>
        /// <param name="head">The first DateTime of the period.</param>
        /// <param name="resolution">The resolution of the period.</param>
        /// <param name="open">The per-share price of the first transaction in the period.</param>
        /// <param name="high">The highest per-share price traded in the period.</param>
        /// <param name="low">The lowest per-share price traded in the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public static IPricePeriod CreateStaticPricePeriod(DateTime head, Resolution resolution, decimal open, decimal high, decimal low, decimal close, long volume)
        {
            return new StaticPricePeriod(head, resolution, open, high, low, close, volume);
        }
    }
}
