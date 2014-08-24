using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface ITimeSeriesFactory<TPeriodValue>
    {
        ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructMutable();

        ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructMutable(IEnumerable<ITimePeriod<TPeriodValue>> timePeriods);

        ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructImmutable(IEnumerable<ITimePeriod<TPeriodValue>> timePeriods);
    }
}