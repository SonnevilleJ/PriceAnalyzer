using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface ITimeSeries<out TSeriesValue, out TPeriodValue> : ITimePeriod<TPeriodValue> where TSeriesValue : ITimePeriod<TPeriodValue>
    {
        IEnumerable<TSeriesValue> TimePeriods { get; }
    }
}