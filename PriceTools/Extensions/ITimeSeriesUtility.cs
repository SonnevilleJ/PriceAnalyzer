using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface ITimeSeriesUtility
    {
        bool HasValueInRange(IPriceSeries priceSeries, DateTime settlementDate);

        IEnumerable<ITimePeriod<decimal>> ResizeTimePeriods(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, Resolution resolution);

        IEnumerable<ITimePeriod<decimal>> ResizeTimePeriods(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, Resolution resolution, DateTime head, DateTime tail);

        ITimePeriod<decimal> GetPreviousTimePeriod(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, DateTime origin);

        IEnumerable<ITimePeriod<decimal>> GetPreviousTimePeriods(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, int maximumCount, DateTime origin);

        IEnumerable<IPricePeriod> GetPreviousPricePeriods(IPriceSeries priceSeries, int maximumCount, DateTime origin);

        IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution);

        IEnumerable<IPricePeriod> ResizePricePeriods(IPriceSeries priceSeries, Resolution resolution, DateTime head, DateTime tail);

        IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution);

        IEnumerable<IPricePeriod> ResizePricePeriods(IEnumerable<IPricePeriod> pricePeriods, Resolution resolution, DateTime head, DateTime tail);
    }
}