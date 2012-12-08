using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Contains extension methods for <see cref="ITimeSeries"/> objects.
    /// </summary>
    public static class TimeSeriesExtensions
    {
        #region TimeSeries Extensions

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        public static IEnumerable<ITimePeriod> ResizeTimePeriods(this ITimeSeries timeSeries, Resolution resolution)
        {
            if (timeSeries.TimePeriods.Any()) return timeSeries.ResizeTimePeriods(resolution, timeSeries.Head, timeSeries.Tail);
            if (resolution < timeSeries.Resolution)
                throw new InvalidOperationException(
                    String.Format(
                        Strings.TimeSeriesExtensions_GetTimePeriods_Unable_to_get_time_periods_using_resolution__0___Best_supported_resolution_is__1__,
                        resolution, timeSeries.Resolution));
            return new List<ITimePeriod>();
        }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this TimeSeries.</exception>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        public static IEnumerable<ITimePeriod> ResizeTimePeriods(this ITimeSeries timeSeries, Resolution resolution, DateTime head, DateTime tail)
        {
            // defer to child object
            var priceSeries = timeSeries as IPriceSeries;
            if (priceSeries != null) return priceSeries.ResizePricePeriods(resolution, head, tail);

            if (resolution < timeSeries.Resolution)
                throw new InvalidOperationException(
                    String.Format(
                        Strings.TimeSeriesExtensions_GetTimePeriods_Unable_to_get_time_periods_using_resolution__0___Best_supported_resolution_is__1__,
                        resolution, timeSeries.Resolution));
            var dataPeriods = timeSeries.TimePeriods.Where(period => period.Head >= head && period.Tail <= tail).OrderBy(period => period.Head).ToList();
            if (resolution == timeSeries.Resolution) return dataPeriods;

            var pairs = GetResolutionDatePairs(resolution, head, tail);

            return (from pair in pairs
                    let periodHead = pair.Key
                    let periodTail = pair.Value
                    let periodsInRange = dataPeriods.Where(period => period.Head >= periodHead && period.Tail <= periodTail).ToList()
                    let value = periodsInRange.Last()[periodTail]
                    select TimePeriodFactory.ConstructTimePeriod(periodHead, periodTail, value));
        }

        /// <summary>
        /// Gets the preceding <see cref="ITimePeriod"/>s previous to an <paramref name="origin" /> date.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="origin">The date of the current period.</param>
        /// <returns></returns>
        public static ITimePeriod GetPreviousTimePeriod(this ITimeSeries timeSeries, DateTime origin)
        {
            return GetPreviousTimePeriods(timeSeries, 1, origin).First();
        }

        /// <summary>
        /// Gets a list of <see cref="ITimePeriod"/>s previous to an <paramref name="origin" /> date.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="maximumCount">The maximum number of periods to select.</param>
        /// <param name="origin">The date which all period tail must precede.</param>
        /// <returns></returns>
        public static IEnumerable<ITimePeriod> GetPreviousTimePeriods(this ITimeSeries timeSeries, int maximumCount, DateTime origin)
        {
            return GetPreviousPeriods(maximumCount, origin, timeSeries.TimePeriods);
        }

        /// <summary>
        /// Gets a list of <see cref="IPricePeriod"/>s previous to an <paramref name="origin" /> date.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="maximumCount">The maximum number of periods to select.</param>
        /// <param name="origin">The date which all period tail must precede.</param>
        /// <returns></returns>
        public static IEnumerable<IPricePeriod> GetPreviousPricePeriods(this IPriceSeries priceSeries, int maximumCount, DateTime origin)
        {
            return GetPreviousPeriods(maximumCount, origin, priceSeries.PricePeriods);
        }

        private static IEnumerable<T> GetPreviousPeriods<T>(int maximumCount, DateTime origin, IEnumerable<T> periods) where T : ITimePeriod
        {
            var previousPeriods = periods.Where(p => p.Tail < origin).ToArray();
            if (previousPeriods.Count() <= maximumCount) return previousPeriods;

            // select most recent periods up to maximumCount
            var results = new List<T>();
            for (var i = 0; i < maximumCount; i++)
            {
                results.Add(previousPeriods[previousPeriods.Count() - (maximumCount - i)]);
            }
            return results;
        }

        #endregion

        #region PriceSeries Extensions

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public static IEnumerable<IPricePeriod> ResizePricePeriods(this IPriceSeries priceSeries, Resolution resolution)
        {
            return priceSeries.PricePeriods.ResizePricePeriods(resolution);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public static IEnumerable<IPricePeriod> ResizePricePeriods(this IPriceSeries priceSeries, Resolution resolution, DateTime head, DateTime tail)
        {
            return priceSeries.PricePeriods.ResizePricePeriods(resolution, head, tail);
        }

        #endregion

        #region IEnumerable<PricePeriod> Extensions

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="pricePeriods"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public static IEnumerable<IPricePeriod> ResizePricePeriods(this IEnumerable<IPricePeriod> pricePeriods, Resolution resolution)
        {
            var periods = pricePeriods.ToArray();
            if (periods.Any()) return periods.ResizePricePeriods(resolution, periods.Min(p => p.Head), periods.Max(p => p.Tail));
            if (resolution < periods.Max(p => p.Resolution))
                throw new InvalidOperationException(
                    String.Format(Strings.PriceSeries_GetPricePeriods_Unable_to_get_price_periods_using_resolution__0___Best_supported_resolution_is__1__,
                                  resolution, periods.Max(p => p.Resolution)));
            return new List<IPricePeriod>();
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="pricePeriods"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public static IEnumerable<IPricePeriod> ResizePricePeriods(this IEnumerable<IPricePeriod> pricePeriods, Resolution resolution, DateTime head, DateTime tail)
        {
            var periods = pricePeriods.ToArray();
            if (resolution < periods.Max(p => p.Resolution))
                throw new InvalidOperationException(
                    String.Format(Strings.PriceSeries_GetPricePeriods_Unable_to_get_price_periods_using_resolution__0___Best_supported_resolution_is__1__,
                                  resolution, periods.Max(p => p.Resolution)));
            var dataPeriods = periods.Where(period => period.Head >= head && period.Tail <= tail).OrderBy(period => period.Head).ToList();
            if (resolution == periods.Max(p => p.Resolution)) return dataPeriods;

            var pairs = GetResolutionDatePairs(resolution, head, tail);

            return (from pair in pairs
                    let periodHead = pair.Key
                    let periodTail = pair.Value
                    let periodsInRange = dataPeriods.Where(period => period.Head >= periodHead && period.Tail <= periodTail).ToList()
                    let open = periodsInRange.First().Open
                    let high = periodsInRange.Max(p => p.High)
                    let low = periodsInRange.Min(p => p.Low)
                    let close = periodsInRange.Last().Close
                    let volume = periodsInRange.Sum(p => p.Volume)
                    select PricePeriodFactory.ConstructStaticPricePeriod(periodHead, periodTail, open, high, low, close, volume));
        }

        #endregion

        private static IEnumerable<KeyValuePair<DateTime, DateTime>> GetResolutionDatePairs(Resolution resolution, DateTime head, DateTime tail)
        {
            if (!head.IsInTradingPeriod(resolution))
            {
                head = head.NextTradingPeriodOpen(resolution);
            }

            var list = new List<KeyValuePair<DateTime, DateTime>>();
            while (head < tail)
            {
                var periodValue = head.CurrentPeriodClose(resolution);
                var lastDay = periodValue > tail ? tail : periodValue;
                list.Add(new KeyValuePair<DateTime, DateTime>(head, lastDay));
                head = lastDay.NextPeriodOpen(resolution);
            }
            return list;
        }
    }
}
