using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools
{
    public class TimeSeriesUtility : ITimeSeriesUtility
    {
        private readonly ITimePeriodFactory<decimal> _timePeriodFactory;

        public TimeSeriesUtility()
        {
            _timePeriodFactory = new TimePeriodFactory<decimal>();
        }

        public bool HasValueInRange(IPriceSeries priceSeries, DateTime settlementDate)
        {
            return priceSeries.PricePeriods.Any() && priceSeries.HasValueInRange(settlementDate);
        }

        public IEnumerable<ITimePeriod<decimal>> ResizeTimePeriods(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, Resolution resolution)
        {
            if (timeSeries.TimePeriods.Any()) return ResizeTimePeriods(timeSeries, resolution, timeSeries.Head, timeSeries.Tail);
            if (resolution < timeSeries.Resolution)
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        Strings.TimeSeriesExtensions_GetTimePeriods_Unable_to_get_time_periods_using_resolution__0___Best_supported_resolution_is__1__,
                        resolution, timeSeries.Resolution));
            return new List<ITimePeriod<decimal>>();
        }

        public IEnumerable<ITimePeriod<decimal>> ResizeTimePeriods(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, Resolution resolution, DateTime head, DateTime tail)
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

        public ITimePeriod<decimal> GetPreviousTimePeriod(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, DateTime origin)
        {
            return GetPreviousTimePeriods(timeSeries, 1, origin).First();
        }

        public IEnumerable<ITimePeriod<decimal>> GetPreviousTimePeriods(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, int maximumCount, DateTime origin)
        {
            return GetPreviousPeriods(maximumCount, origin, timeSeries.TimePeriods);
        }

        public IEnumerable<IPricePeriod> GetPreviousPricePeriods(IPriceSeries priceSeries, int maximumCount, DateTime origin)
        {
            return GetPreviousPeriods(maximumCount, origin, priceSeries.PricePeriods);
        }

        private IEnumerable<T> GetPreviousPeriods<T>(int maximumCount, DateTime origin, IEnumerable<T> periods) where T : IVariableValue<decimal>
        {
            var previousPeriods = periods.Where(p => p.Tail < origin).ToArray();
            if (previousPeriods.Count() <= maximumCount) return previousPeriods;

            // select most recent periods up to maximumCount
            return PreviousPeriods(maximumCount, previousPeriods);
        }

        private IEnumerable<T> PreviousPeriods<T>(int maximumCount, T[] previousPeriods) where T : IVariableValue<decimal>
        {
            for (var i = 0; i < maximumCount; i++)
            {
                yield return previousPeriods.ElementAt(previousPeriods.Count() - (maximumCount - i));
            }
        }

        public IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution)
        {
            return ResizePricePeriods(priceSeries.PricePeriods, resolution);
        }

        public IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution, DateTime head, DateTime tail)
        {
            return ResizePricePeriods(priceSeries.PricePeriods, resolution, head, tail);
        }

        public IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution)
        {
            var periods = pricePeriods.ToArray();
            if (periods.Any()) return ResizePricePeriods(periods, resolution, periods.Min(p => p.Head), periods.Max(p => p.Tail));
            ValidateResolution(resolution, periods.Max(p => p.Resolution));
            return new List<IPricePeriod>();
        }

        public IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution, DateTime head, DateTime tail)
        {
            var periods = pricePeriods.ToArray();
            ValidateResolution(resolution, periods.Max(p => p.Resolution));
            var dataPeriods = periods.Where(period => period.Head >= head && period.Tail <= tail).OrderBy(period => period.Head).ToList();
            if (resolution == periods.Max(p => p.Resolution)) return dataPeriods;

            var pairs = GetResolutionDatePairs(resolution, head, tail);

            var resized = from pair in pairs
                let periodHead = pair.Key
                let periodTail = pair.Value
                let periodsInRange = dataPeriods.Where(period => period.Head >= periodHead && period.Tail <= periodTail).ToList()
                let open = periodsInRange.First().Open
                let high = periodsInRange.Max(p => p.High)
                let low = periodsInRange.Min(p => p.Low)
                let close = periodsInRange.Last().Close
                let volume = periodsInRange.Sum(p => p.Volume)
                select new PricePeriod(periodHead, periodTail, open, high, low, close, volume);
            return resized.Cast<IPricePeriod>();
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
