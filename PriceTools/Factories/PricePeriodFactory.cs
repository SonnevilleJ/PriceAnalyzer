using System;
using System.Globalization;
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
        /// <param name="resolution">The resolution of the period.</param>
        /// <param name="open">The per-share price of the first transaction in the period.</param>
        /// <param name="high">The highest per-share price traded in the period.</param>
        /// <param name="low">The lowest per-share price traded in the period.</param>
        /// <param name="close">The per-share price of the last transaction in the period.</param>
        /// <param name="volume">The total number of shares traded during the period.</param>
        /// <returns>A PricePeriod object with only a close.</returns>
        public IPricePeriod ConstructStaticPricePeriod(DateTime head, Resolution resolution, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null)
        {
            return ConstructStaticPricePeriod(head, ConstructTail(head, resolution), open, high, low, close, volume);
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
        public IPricePeriod ConstructStaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
        {
            if (open.HasValue && open.Value < 0)
                throw new ArgumentOutOfRangeException("open", open, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Opening_price_must_be_greater_than_or_equal_to_zero_);
            if (high.HasValue && high.Value < 0)
                throw new ArgumentOutOfRangeException("high", high, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_High_price_must_be_greater_than_or_equal_to_zero_);
            if (low.HasValue && low.Value < 0)
                throw new ArgumentOutOfRangeException("low", low, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Low_price_must_be_greater_than_or_equal_to_zero_);
            if (close < 0)
                throw new ArgumentOutOfRangeException("close", close, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Closing_price_must_be_greater_than_or_equal_to_zero_);
            if (volume.HasValue && volume.Value < 0)
                throw new ArgumentOutOfRangeException("volume", volume, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Volume_must_be_greater_than_or_equal_to_zero_);
            if (head > tail)
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Head_must_come_before_Tail_);
            if (high < open)
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Opening_price_cannot_be_higher_than_High_price_);
            if (high < close)
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Closing_price_cannot_be_higher_than_High_price_);
            if (low > open)
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Opening_price_cannot_be_lower_than_Low_price_);
            if (low > close)
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Closing_price_cannot_be_lower_than_Low_price_);

            return new PricePeriod(head, tail, open ?? close, high ?? close, low ?? close, close, volume);
        }

        private static DateTime ConstructTail(DateTime head, Resolution resolution)
        {
            var result = head;
            switch (resolution)
            {
                case Resolution.Days:
                    result = head.AddDays(1);
                    break;
                case Resolution.Weeks:
                    switch (result.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            result = result.AddDays(5);
                            break;
                        case DayOfWeek.Tuesday:
                            result = result.AddDays(4);
                            break;
                        case DayOfWeek.Wednesday:
                            result = result.AddDays(3);
                            break;
                        case DayOfWeek.Thursday:
                            result = result.AddDays(2);
                            break;
                        case DayOfWeek.Friday:
                            result = result.AddDays(1);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("resolution", resolution, String.Format(CultureInfo.InvariantCulture, Strings.StaticPricePeriodImpl_ConstructTail_Unable_to_identify_the_period_tail_for_resolution__0__, resolution));
            }
            return result.Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }
    }
}
