using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Contains extension methods for <see cref="ITimeSeries"/> objects.
    /// </summary>
    public class TimeSeriesUtility : ITimeSeriesUtility
    {
        private readonly IPricePeriodFactory _pricePeriodFactory;
        private readonly ITimePeriodFactory _timePeriodFactory;

        /// <summary>
        /// Constructs a TimeSeriesUtility.
        /// </summary>
        public TimeSeriesUtility()
        {
            _pricePeriodFactory = new PricePeriodFactory();
            _timePeriodFactory = new TimePeriodFactory();
        }

        /// <summary>
        /// Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to inspect.</param>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimePeriod has a valid value for the given date.</returns>
        public bool HasValueInRange(ITimeSeries<ITimePeriod> timeSeries, DateTime settlementDate)
        {
            if (!timeSeries.TimePeriods.Any()) return false;
            return (timeSeries as ITimePeriod).HasValueInRange(settlementDate);
        }

        /// <summary>
        /// Determines if the IPriceSeries has a valid value for a given date.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to inspect.</param>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the IPricePeriod has a valid value for the given date.</returns>
        public bool HasValueInRange(IPriceSeries priceSeries, DateTime settlementDate)
        {
            if (!priceSeries.PricePeriods.Any()) return false;
            return HasValueInRange((priceSeries as ITimeSeries<ITimePeriod>), settlementDate);
        }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        public IEnumerable<ITimePeriod> ResizeTimePeriods(ITimeSeries<ITimePeriod> timeSeries, Resolution resolution)
        {
            if (timeSeries.TimePeriods.Any()) return ResizeTimePeriods(timeSeries, resolution, timeSeries.Head, timeSeries.Tail);
            if (resolution < timeSeries.Resolution)
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
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
        public IEnumerable<ITimePeriod> ResizeTimePeriods(ITimeSeries<ITimePeriod> timeSeries, Resolution resolution, DateTime head, DateTime tail)
        {
            // defer to child object
            var priceSeries = timeSeries as IPriceSeries;
            if (priceSeries != null) return ResizePricePeriods(priceSeries, resolution, head, tail);

            ValidateResolution(resolution, timeSeries.Resolution);
            var dataPeriods = timeSeries.TimePeriods.Where(period => period.Head >= head && period.Tail <= tail).OrderBy(period => period.Head).ToList();
            if (resolution == timeSeries.Resolution) return dataPeriods;

            var pairs = GetResolutionDatePairs(resolution, head, tail);

            return (from pair in pairs
                    let periodHead = pair.Key
                    let periodTail = pair.Value
                    let periodsInRange = dataPeriods.Where(period => period.Head >= periodHead && period.Tail <= periodTail).ToList()
                    let value = periodsInRange.Last()[periodTail]
                    select _timePeriodFactory.ConstructTimePeriod(periodHead, periodTail, value));
        }

        /// <summary>
        /// Gets the preceding <see cref="ITimePeriod"/>s previous to an <paramref name="origin" /> date.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="origin">The date of the current period.</param>
        /// <returns></returns>
        public ITimePeriod GetPreviousTimePeriod(ITimeSeries<ITimePeriod> timeSeries, DateTime origin)
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
        public IEnumerable<ITimePeriod> GetPreviousTimePeriods(ITimeSeries<ITimePeriod> timeSeries, int maximumCount, DateTime origin)
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
        public IEnumerable<IPricePeriod> GetPreviousPricePeriods(IPriceSeries priceSeries, int maximumCount, DateTime origin)
        {
            return GetPreviousPeriods(maximumCount, origin, priceSeries.PricePeriods);
        }

        private IEnumerable<T> GetPreviousPeriods<T>(int maximumCount, DateTime origin, IEnumerable<T> periods) where T : IVariableValue
        {
            var previousPeriods = periods.Where(p => p.Tail < origin).ToArray();
            if (previousPeriods.Count() <= maximumCount) return previousPeriods;

            // select most recent periods up to maximumCount
            return PreviousPeriods(maximumCount, previousPeriods);
        }

        private IEnumerable<T> PreviousPeriods<T>(int maximumCount, T[] previousPeriods) where T : IVariableValue
        {
            for (var i = 0; i < maximumCount; i++)
            {
                yield return previousPeriods.ElementAt(previousPeriods.Count() - (maximumCount - i));
            }
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution)
        {
            return ResizePricePeriods(priceSeries.PricePeriods, resolution);
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
        public IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution, DateTime head, DateTime tail)
        {
            return ResizePricePeriods(priceSeries.PricePeriods, resolution, head, tail);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="pricePeriods"></param>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution)
        {
            var periods = pricePeriods.ToArray();
            if (periods.Any()) return ResizePricePeriods(periods, resolution, periods.Min(p => p.Head), periods.Max(p => p.Tail));
            ValidateResolution(resolution, periods.Max(p => p.Resolution));
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
        public IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution, DateTime head, DateTime tail)
        {
            var periods = pricePeriods.ToArray();
            ValidateResolution(resolution, periods.Max(p => p.Resolution));
            var dataPeriods = periods.Where(period => period.Head >= head && period.Tail <= tail).OrderBy(period => period.Head).ToList();
            if (resolution == periods.Max(p => p.Resolution)) return dataPeriods;

            var pairs = GetResolutionDatePairs(resolution, head, tail);

            return from pair in pairs
                   let periodHead = pair.Key
                   let periodTail = pair.Value
                   let periodsInRange = dataPeriods.Where(period => period.Head >= periodHead && period.Tail <= periodTail).ToList()
                   let open = periodsInRange.First().Open
                   let high = periodsInRange.Max(p => p.High)
                   let low = periodsInRange.Min(p => p.Low)
                   let close = periodsInRange.Last().Close
                   let volume = periodsInRange.Sum(p => p.Volume)
                   select _pricePeriodFactory.ConstructStaticPricePeriod(periodHead, periodTail, open, high, low, close, volume);
        }

        private void ValidateResolution(Resolution desiredResolution, Resolution actualResolution)
        {
            if (desiredResolution < actualResolution)
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        Strings.PriceSeries_GetPricePeriods_Unable_to_get_price_periods_using_resolution__0___Best_supported_resolution_is__1__,
                        desiredResolution,
                        actualResolution
                        )
                    );
        }

        private IEnumerable<KeyValuePair<DateTime, DateTime>> GetResolutionDatePairs(Resolution resolution, DateTime head, DateTime tail)
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
